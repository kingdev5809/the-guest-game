namespace TheGame.Models
{
    public class ComplatedGame
    {
        public int? Id { get; set; }
        public Guid GameId { get; set; }
        public int? M { get; set; }
        public int? P { get; set; }
        public int? Tries { get; set; }
        public bool Win { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
