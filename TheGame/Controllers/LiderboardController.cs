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

        [HttpGet("all")]
        public async Task<IActionResult> GetAllComplatedGames()
        {
            try
            {
                var data = await _repository.GetLiderboardAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("win")]
        public async Task<IActionResult> GetAllComplatedGamesSortedByWinEffortAsync()
        {
            try
            {
                var data = await _repository.GetLiderboardSortedByWinEffortAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       

        [HttpGet("num")]
        public async Task<IActionResult> GetAllComplatedGamesSortedByAbundanceOfNumbersAsync()
        {
            try
            {
                var data = await _repository.GetLiderboardSortedByAbundanceOfNumbersAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
