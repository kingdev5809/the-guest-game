namespace TheGame.Models
{
    public class UserStats
    {
        public User User { get; set; }
        public int GamesPlayed { get; set; }
        public int?GamesWon { get; set; }
        public int?TotalTries { get; set; }

        public double SuccessRate => (double)GamesWon / GamesPlayed;
    }
}
