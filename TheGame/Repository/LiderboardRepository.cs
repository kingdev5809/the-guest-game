using Microsoft.EntityFrameworkCore;
using TheGame.Data;
using TheGame.Models;

namespace TheGame.Repository
{
    public class LiderboardRepository
    {
        private readonly InMemoryDbContext _context;

        public LiderboardRepository(InMemoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ComplatedGame>> GetLiderboardAsync()
        {
            return await _context.ComplatedGames.Include(m => m.User).ToListAsync();
        }

        public async Task<IEnumerable<ComplatedGame>> GetLiderboardSortedByWinEffortAsync()
        {
            var completedGames = await GetLiderboardAsync();
            completedGames = completedGames.OrderBy(g => g.Tries)
                                           .ThenBy(g => g.Win);
                                        
            return completedGames;
        }     

        public async Task<IEnumerable<ComplatedGame>> GetLiderboardSortedByAbundanceOfNumbersAsync()
        {
            var completedGames = await GetLiderboardAsync();          
            completedGames = completedGames.OrderByDescending(g => g.P)
                                            .ThenBy(g => g.Tries)
                                           .ThenByDescending(g => g.M);
            return completedGames;
        }
    }
}
