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

    

        public async Task<List<UserStats>> GetLeaderboardWithGameCount(int minGamesPlayed)
        {
            var users = await _context.Users.ToListAsync();

            var userStats = users
                .Select(u => new UserStats
                {
                    User = u,
                    GamesPlayed = _context.ComplatedGames.Count(g => g.UserId == u.Id),
                    GamesWon = _context.ComplatedGames.Count(g => g.UserId == u.Id && g.Win),
                    TotalTries = _context.ComplatedGames.Where(g => g.UserId == u.Id).Sum(g => g.Tries)
                })
                .Where(us => us.GamesPlayed >= minGamesPlayed)
                .ToList();  // Perform calculations in memory

            var sortedUserStats = userStats
                .OrderByDescending(us => us.SuccessRate)
                .ThenBy(us => us.TotalTries)
                .ToList();

            return sortedUserStats;
        }

        public async Task<List<UserStats>> GetLeaderboard()
        {
            var users = await _context.Users.ToListAsync();

            var userStats = users
                .Select(u => new UserStats
                {
                    User = u,
                    GamesPlayed = _context.ComplatedGames.Count(g => g.UserId == u.Id),
                    GamesWon = _context.ComplatedGames.Count(g => g.UserId == u.Id && g.Win),
                    TotalTries = _context.ComplatedGames.Where(g => g.UserId == u.Id).Sum(g => g.Tries)
                })             
                .ToList();  // Perform calculations in memory

            var sortedUserStats = userStats
                .OrderByDescending(us => us.SuccessRate)
                .ThenBy(us => us.TotalTries)
                .ToList();

            return sortedUserStats;
        }

    }
}
