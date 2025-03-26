using System.Globalization;
using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Interfaces;
using TheatricalPlayersRefactoringKata.Domain.Models;
using TheatricalPlayersRefactoringKata.Domain.Services.Factories;

namespace TheatricalPlayersRefactoringKata.Domain.Services.Adapters
{
    public class StatementToTextAdapter : IStatementAdapter
    {
        public async Task<string> PrintAsync(Invoice invoice, Dictionary<string, Play> plays, CultureInfo cultureInfo)
        {
            var totalAmount = decimal.Zero;
            var volumeCredits = 0;

            using var writer = new StringWriter();

            await writer.WriteLineAsync($"Statement for {invoice.Customer}");

            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayId];
                var lines = play.Lines;
                decimal thisAmount;

                var genre = GenreFactory.Create(play.Type, perf.Audience, play.Lines);

                thisAmount = genre.CalculateAmount();
                volumeCredits += genre.CalculateCredit();

                await writer.WriteLineAsync(string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)", play.Name, thisAmount, perf.Audience));
                totalAmount += thisAmount;
            }

            await writer.WriteLineAsync(string.Format(cultureInfo, "Amount owed is {0:C}", totalAmount));
            await writer.WriteLineAsync($"You earned {volumeCredits} credits");

            return writer.ToString();
        }
    }
}
