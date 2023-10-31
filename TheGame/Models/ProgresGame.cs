namespace TheGame.Models
{
    public class ProgresGame
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public int? M { get; set; } = 0;
        public int? P { get; set; } = 0;
        public int? Tries { get; set; } = 0;
        public string?  RandomNumber { get; set; }
        public string? ClientNumber { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
        public bool Win { get; set; }
        public string? Message { get; set; }

    }

  
}
