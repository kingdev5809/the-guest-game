namespace TheGame.Models
{
    public class TryHistory
    {
        public int Id { get; set; }
        public Guid GameId { get; set; }
        public int? TryNumber { get; set; }
        public string?ClientNum { get; set; }
        public int? M { get; set; }
        public int? P { get; set; }
    }
}
