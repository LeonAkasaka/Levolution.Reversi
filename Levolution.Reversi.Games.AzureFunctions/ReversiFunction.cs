using Levolution.Reversi.Records;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Levolution.Reversi.Games.AzureFunctions
{
    public static class ReversiFunction
    {
        [FunctionName("Function")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var table = new Table();
            var player = Player.Dark;

            string record = req.Query["record"];
            if (!string.IsNullOrEmpty(record))
            {
                var pts = CellPosition.ParseList(record.ToLower().Trim());
                player = table.Reset(pts);
            }
            else { table.Reset(); }

            var random = new Random();
            var placableCells = table.GetPlaceableCells(player).ToArray();
            if (placableCells.Length == 0)
            {
                // pass or gameover.
                return new OkObjectResult(record);
            }

            var index = random.Next(placableCells.Length);
            var next = placableCells[index];
            if (table.TryPlace(next, player))
            {
                return new OkObjectResult(record + next.ToString());
            }
            else
            {
                // TODO: Error
                return new EmptyResult();
            }
            
        }
    }
}
