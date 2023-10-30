namespace TheGame.Models
{
    public class ResponseModel
    {
        public string clientNumber { get; set; }
        public Guid GameId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
