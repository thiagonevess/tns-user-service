using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserService.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserDbContext dbContext;

        public UserController(UserDbContext dbContext)
        {
            this.dbContext = dbContext;

            if (this.dbContext.Users.Count() == 0)
            {
                User user = new User { Id = 1, Name = "Thiago" };
                this.dbContext.Users.Add(user);
                this.dbContext.SaveChanges();
            }
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await this.dbContext.Users.ToListAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var todoItem = await this.dbContext.Users.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            this.dbContext.Users.Add(user);
            await this.dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id, name = user.Name }, user);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(long id, User user)
        {
            if (id != user.Id) return BadRequest();

            this.dbContext.Entry(user).State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync();
            return NoContent();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var user = await this.dbContext.Users.FindAsync(id);
            if (user == null) return NotFound();
            this.dbContext.Users.Remove(user);
            await this.dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
