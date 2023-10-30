namespace TheGame.Models
{
    public class Result
    {
        public Guid Id { get; set; }
        public string? RandomNumber { get; set; }
        public string? Message { get; set; }
        public int? M { get; set; }
        public int? P { get; set; }
        public int? Tries { get; set; }

    }
}
