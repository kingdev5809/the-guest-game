namespace TheGame.Models
{
    public class CheckGameViewModel
    {
        public ProgresGame LastGame { get; set; }
        public List<TryHistory> Histories { get; set; }
    }
}
