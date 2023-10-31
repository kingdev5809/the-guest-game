using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheGame.Repository;

namespace TheGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiderboardController : ControllerBase
    {
        private readonly LiderboardRepository _repository;

        public LiderboardController(LiderboardRepository repository)
        {
            _repository = repository;
        }
  
        [HttpGet("{minGamesPlayed}")]
        public async Task<IActionResult> GetLiderboardWithGameCount(int minGamesPlayed)
        {
            try
            {
                var data = await _repository.GetLeaderboardWithGameCount(minGamesPlayed);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLeaderboard()
        {
            try
            {
                var data = await _repository.GetLeaderboard();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
