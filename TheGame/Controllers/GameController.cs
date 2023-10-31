using Microsoft.AspNetCore.Mvc;
using TheGame.Models;
using TheGame.Repository;

namespace TheGame.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameRepository _repository;
        public GameController(GameRepository repository)
        {
           _repository = repository;
        }

        [HttpGet("start")]
        public IActionResult StartGame()
        {
            var data =    _repository.StartGame();
            return Ok(data);
        }

        [HttpPost("check")]
        public async Task<IActionResult> CheckGame([FromBody] ResponseModel response)
        {
            try
            {
                var data = await _repository.CheckGame(response);                      
                return Ok(data);
            }
            catch(Exception ex) { 
                return BadRequest(ex.Message);
              }           
        }

    
    }
}
