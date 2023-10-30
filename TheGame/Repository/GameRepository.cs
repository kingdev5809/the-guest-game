using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using TheGame.Data;
using TheGame.Interface;
using TheGame.Models;
using TheGame.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TheGame.Repository
{
    public class GameRepository 
    {

        EqResult eqResult = new EqResult();
        private readonly InMemoryDbContext _context;
        private readonly GameService _gameService;

        public GameRepository(InMemoryDbContext context, GameService gameService)
        {
            _context = context;
            _gameService = gameService;
        }    


        public RandomNumber StartGame()
        {
           var randomNumber= _gameService.GenerateUniqueRandomNumbers();
            RandomNumber numbers = new RandomNumber();
            numbers.Number = randomNumber.ToString();
            numbers.GameId = Guid.NewGuid();
            _context.RandomNumbers.Add(numbers);
            _context.SaveChanges();
            return numbers;
        }

        public async Task<ProgresGame> CheckGame(ResponseModel response)
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

            // Get the existing game record, if any
            var existedGame = await _context.ProgresGames.FirstOrDefaultAsync(n => n.GameId == response.GameId);
            if (existedGame !=null)

            {
                if (existedGame.Win == true || existedGame.Tries == 8)
                {
                    throw new Exception("The Game Already Complated. Start Another Game");
                }
            }

            progresGame.RandomNumber = randomNumber.Number;
            progresGame.ClientNumber = response.clientNumber;

            // Calculate the results of the game
            var result  =   _gameService.CheckClientNumbers(progresGame);
     

            // Add or update the game record
            ProgresGame gameRes = _gameService.AddOrUpdateGames(progresGame,  response);
            gameRes.P = result.P;
            gameRes.M = result.M;
            gameRes.UserId = response.UserId;
            if (existedGame == null)
            {
                _context.ProgresGames.Add(gameRes);
            }
            else
            {
                gameRes.Tries += existedGame.Tries++;
                existedGame = gameRes;
            }

            // Create response message for client side
            ProgresGame createResponse =  _gameService.CreateResponse(gameRes);

            // Complate game. If tries = 8 or client win the game. Game is Complated and add the database
            if (createResponse.Win  == true  ||   createResponse.Tries == 8 )
            {
                ComplatedGame complatedGame = _gameService.GeneradeComplateGame(createResponse);
                 _context.ComplatedGames.Add(complatedGame);
            }
            _context.SaveChanges();
            return createResponse;
        }

     
        public async Task<List<ComplatedGame>> GetComplatedGames()
        {
            var data = await _context.ComplatedGames.ToListAsync();
            return data;
        }

    }
}


