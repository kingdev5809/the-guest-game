using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheGame.Data;
using TheGame.Models;

namespace TheGame.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly InMemoryDbContext _context;

        public UserController(InMemoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult GetUsers()
        {
            var users = _context.Users.ToListAsync();
            return Ok(users);
        }
        [HttpGet("{Id}")]
           public async Task<ActionResult> GetOneUser(int Id)
    {
            var user = _context.Users.FindAsync(Id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
    }
        [HttpPost("register")]
        public async Task<ActionResult> Register(User user)
        {
            var userExist = await _context.Users.FirstOrDefaultAsync(u => u.Name == user.Name);
            if (userExist != null)
            {
                return Conflict("The user name already taken");
            }
            var data = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return  Ok(user) ;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(User user)
        {
            var userExist = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (userExist == null)
            {
                return NotFound("User not found");
            }
            if (userExist.Password != user.Password)
            {
                return BadRequest("Password is incorrect!");
            }
            return Ok(userExist);
        }

        [HttpPost("edit")]
        public async Task<ActionResult> EditUser (User user)
        {
            var userExist = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (userExist == null)
            {
                return NotFound("User not found");
            }

            userExist.Name = user.Name;
            userExist.Password = user.Password;
            await _context.SaveChangesAsync();
            return Ok(userExist);
        }
    }
}
