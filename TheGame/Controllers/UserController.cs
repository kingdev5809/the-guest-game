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
        [HttpPost]
        public async Task<ActionResult> PostUser(User user)
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


    }
}
