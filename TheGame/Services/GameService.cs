using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TheGame.Data;
using TheGame.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TheGame.Services
{
    public class GameService
    {
       
        public ProgresGame CheckClientNumbers(ProgresGame progresGame)
        {
            char[] arrayRandomNumber = progresGame.RandomNumber.ToCharArray();
            char[] arrayClientNumber = progresGame.ClientNumber.ToCharArray();           
            for (int index = 0; index < progresGame.RandomNumber.Length; index++)
            {
                if (arrayRandomNumber[index] == arrayClientNumber[index])
                {
                    progresGame.P++;
                }
                else if (progresGame.ClientNumber.Contains(arrayRandomNumber[index]))
                {
                    progresGame.M++;
                }
            }
            return progresGame;
        }

        public string GenerateUniqueRandomNumbers()
        {
            var random = new Random();
            var digits = Enumerable.Range(1, 9).OrderBy(x => random.Next()).Take(4);
            return string.Concat(digits);
        }

        public ProgresGame AddOrUpdateGames(ProgresGame progresGame, ResponseModel response)
        {
            progresGame.GameId = response.GameId;
            progresGame.Tries = 1;
            progresGame.Id = Guid.NewGuid();          
            return progresGame;


        }
        
        public ProgresGame CreateResponse(ProgresGame gameRes)
        {
            if (gameRes.P == 4)
            {
                gameRes.Message = "Great You won this game";
                gameRes.Win = true;
                return gameRes;
            }
            else if (gameRes.Tries  == 8)
            {                
                gameRes.Message = $"Sorry, You missed it! Game Lost. Correct with place {gameRes.P} and Correct without place {gameRes.M}.Try play new game";
                return gameRes;
            }
            gameRes.Message = $"Sorry, You missed it! Try again . Correct with place {gameRes.P} and Correct without place {gameRes.M} . Your tried {gameRes.Tries} times";
            return gameRes;
        }

        public ComplatedGame GeneradeComplateGame(ProgresGame progresGame)
        {
            ComplatedGame complatedGame = new ComplatedGame();
            complatedGame.Tries = progresGame.Tries;
            complatedGame.M = progresGame.M;
            complatedGame.P = progresGame.P;
            complatedGame.UserId = progresGame.UserId;
            complatedGame.GameId = progresGame.GameId;
            complatedGame.Win = progresGame.Win;
            return complatedGame;
        }

        public async Task<string> ValidateNumber(string clientNumber)
        {
            if (!Regex.IsMatch(clientNumber, @"^\d{4}$"))
            {             
                return "Client number must be a 4-digit number.";
            }

            // Check if there are any duplicate numbers
            var digits = clientNumber.ToCharArray();
            var distinctDigits = digits.Distinct().ToArray();
            if (distinctDigits.Length == 1)
            {
                return "Client number cannot have duplicate numbers.";
            }         
            return "true";
        }
    }
}
