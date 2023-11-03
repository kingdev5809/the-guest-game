
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

            // Validate the client number
            var validationResult = await _gameService.ValidateNumber(response.clientNumber);
            if (validationResult != "true")
            {
                throw new Exception(validationResult);
            }
            var progresGame = new ProgresGame();


            // Get the random number for the game
            var randomNumber = await _context.RandomNumbers.FirstOrDefaultAsync(h => h.GameId == response.GameId);

            if (randomNumber == null)
            {
                throw new Exception("The Game not found. Start Another Game");
            }
            progresGame.RandomNumber = randomNumber.Number;

            // Get the existing game record, if any
            var existedGame = await _context.ProgresGames.FirstOrDefaultAsync(n => n.GameId == response.GameId);


            if (existedGame != null && (existedGame.Win || existedGame.Tries == 8))
            {
                throw new Exception("The Game Already Complated. Start Another Game");
            }

            //  Calculate the results of the game and  Add  the game record
            progresGame = _gameService.CalculateAndCreateGames(progresGame, response);

            if (existedGame == null)
            {
                AddNewGame(progresGame);
            }
            else
            {

                UpdateExistingGame(existedGame, progresGame);
            }
             CreateHistory(progresGame);
            await _context.SaveChangesAsync();
            // Create response message for client side
            ProgresGame createResponse = _gameService.CreateResponse(progresGame);

            // get all tries the game
            var tryHistory = await _context.TryHistories
                 .Where(h => h.GameId == response.GameId)
                .ToListAsync();



            // Complate game. If tries = 8 or client win the game. Game is Complated and add the database
            if (createResponse.Win == true || createResponse.Tries == 8)
            {
                ComplatedGame complatedGame = _gameService.GeneradeComplateGame(createResponse);
                _context.ComplatedGames.Add(complatedGame);
            }
            _context.SaveChanges();
            checkGameViewModel.LastGame = createResponse;
            checkGameViewModel.Histories = tryHistory;
            return checkGameViewModel;
        }


        private void AddNewGame(ProgresGame progresGame)
        {
            _context.ProgresGames.Add(progresGame);
        }

        private void UpdateExistingGame(ProgresGame existedGame, ProgresGame progresGame)
        {
            existedGame.ClientNumber = progresGame.ClientNumber;
         
            existedGame.M = progresGame.M;
            existedGame.P = progresGame.P;
            existedGame.RandomNumber = progresGame.RandomNumber;
            existedGame.Tries++;
            progresGame.Tries = existedGame.Tries;
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
        }

    }
}

