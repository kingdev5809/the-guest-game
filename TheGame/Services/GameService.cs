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
       
        public ProgresGame CalculateClientNumbers(ProgresGame progresGame)
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

  

 
        public ProgresGame CreateResponse(ProgresGame progresGame)
        {
            if (progresGame.P == 4)
            {
                progresGame.Message = "Great You won this game";
                progresGame.Win = true;
                return progresGame;
            }
            else if (progresGame.Tries  == 8)
            {                
                progresGame.Message = $"Sorry, You missed it! Game Lost. Correct with place {progresGame.P} and Correct without place {progresGame.M}.Try play new game";
                return progresGame;
            }
            progresGame.Message = $"Sorry, You missed it! Try again . Correct with place {progresGame.P} and Correct without place {progresGame.M} . Your tried {progresGame.Tries} times";
            return progresGame;
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
