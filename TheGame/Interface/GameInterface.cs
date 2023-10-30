using TheGame.Models;

namespace TheGame.Interface
{
    public interface GameInterface
    {
        public List<RandomNumber> GenerateRandomNumbers();
        public List<RandomNumber> StartGame();
    }
}
