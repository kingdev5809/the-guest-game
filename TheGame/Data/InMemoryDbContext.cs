using Microsoft.EntityFrameworkCore;
using TheGame.Models;

namespace TheGame.Data
{
    public class InMemoryDbContext : DbContext
    {
        public InMemoryDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users{ get; set; }
        public DbSet<ComplatedGame> ComplatedGames { get; set; }
        public DbSet<ProgresGame> ProgresGames { get; set; }
        public DbSet<RandomNumber> RandomNumbers { get; set; }
        public DbSet<TryHistory> TryHistories { get; set; }
    }

}
