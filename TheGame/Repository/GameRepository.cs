
using Microsoft.EntityFrameworkCore;
using TheGame.Data;
using TheGame.Models;
using TheGame.Services;


namespace TheGame.Repository
{
    public class GameRepository
    {

        CheckGameViewModel checkGameViewModel = new CheckGameViewModel();
        private readonly InMemoryDbContext _context;
        private readonly GameService _gameService;

        public GameRepository(InMemoryDbContext context, GameService gameService)
        {
            _context = context;
            _gameService = gameService;
        }


        public RandomNumber StartGame()
        {
            var randomNumber = _gameService.GenerateUniqueRandomNumbers();
            RandomNumber numbers = new RandomNumber();
            numbers.Number = randomNumber.ToString();
            numbers.GameId = Guid.NewGuid();
            _context.RandomNumbers.Add(numbers);
            _context.SaveChanges();
            return numbers;
        }

        public async Task<CheckGameViewModel> CheckGame(ResponseModel response)
        {
            var progresGame = new ProgresGame();

            // Validate the client number
            var validationResult = await _gameService.ValidateNumber(response.clientNumber);
            if (validationResult != "true")
            {
                throw new Exception(validationResult);
            }


            // Get the existing game record, if any
            var existedGame = await _context.ProgresGames.FirstOrDefaultAsync(n => n.GameId == response.GameId);

            if (existedGame != null && (existedGame.Win || existedGame.Tries == 8))
            {
                throw new Exception("The Game Already Complated. Start Another Game");
            }


            // Get the random number for the game
            var randomNumber = await _context.RandomNumbers.FirstOrDefaultAsync(h => h.GameId == response.GameId);

            if (randomNumber == null)
            {
                throw new Exception("The Game not found. Start Another Game");
            }
            progresGame.RandomNumber = randomNumber.Number;
            progresGame.ClientNumber = response.clientNumber;

     
         


            //  Calculate the results of the game and  Add  the game record
             _gameService.CalculateClientNumbers(progresGame);

            // maping all values and add or update
             AddOrUpdateGame(existedGame , progresGame, response);

            // Create history
             CreateHistory(progresGame);



            // Create response message for client side
            _gameService.CreateResponse(progresGame);
            await _context.SaveChangesAsync();

            // get all history  this game
            var tryHistory = await _context.TryHistories
                   .Where(h => h.GameId == response.GameId)
                  .ToListAsync();


            // Complate game. If tries = 8 or client win the game. Game is Complated and add the database
            if (progresGame.Win == true || progresGame.Tries == 8)
            {
                GeneradeComplateGame(progresGame);               
            }

            await _context.SaveChangesAsync();
            checkGameViewModel.LastGame = progresGame;
            checkGameViewModel.Histories = tryHistory;
            return checkGameViewModel;
        }






        private async void AddOrUpdateGame(ProgresGame existedGame, ProgresGame progresGame, ResponseModel response)
        {            
                progresGame.UserId = response.UserId;
                progresGame.GameId = response.GameId;
                progresGame.Tries++;           
                
            if (existedGame == null)
            {
                progresGame.Id = Guid.NewGuid();
                _context.ProgresGames.Add(progresGame);
                return;
            }
            existedGame.Tries++;
            progresGame.Tries = existedGame.Tries;
            existedGame.Win = progresGame.Win;
            _context.ProgresGames.Update(existedGame);
        }

   

        private void CreateHistory( ProgresGame progresGame)
        {
            var tryHistory = new TryHistory
            {
                GameId = progresGame.GameId,
                TryNumber = progresGame.Tries,
                ClientNum = progresGame.ClientNumber,
                M = progresGame.M,
                P = progresGame.P,
            };
            _context.TryHistories.Add(tryHistory);
            return;
        }

        private void GeneradeComplateGame(ProgresGame progresGame)
        {
            ComplatedGame complatedGame = new ComplatedGame();
            complatedGame.Tries = progresGame.Tries;
            complatedGame.M = progresGame.M;
            complatedGame.P = progresGame.P;
            complatedGame.UserId = progresGame.UserId;
            complatedGame.GameId = progresGame.GameId;
            complatedGame.Win = progresGame.Win;
            _context.ComplatedGames.Add(complatedGame);
        }
    }
}

