using System.Globalization;
using System.Text;
using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Enums;
using TheatricalPlayersRefactoringKata.Domain.Interfaces;
using TheatricalPlayersRefactoringKata.Domain.Models;
using TheatricalPlayersRefactoringKata.Domain.Models.Genres;
using TheatricalPlayersRefactoringKata.Domain.Services.Factories;

namespace TheatricalPlayersRefactoringKata.Domain.Services.Adapters
{
    public class StatementToTextAdapter : IStatementAdapter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays, CultureInfo cultureInfo)
        {
            var totalAmount = decimal.Zero;
            var volumeCredits = 0;

            var statement = new StringBuilder();

            statement.AppendLine(string.Format("Statement for {0}", invoice.Customer));

            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayId];
                var lines = play.Lines;
                decimal thisAmount;

                var genre = GenreFactory.Create(play.Type, perf.Audience, play.Lines);

                thisAmount = genre.CalculateAmount();
                volumeCredits += genre.CalculateCredit();

                statement.AppendLine(string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)", play.Name, thisAmount, perf.Audience));
                totalAmount += thisAmount;
            }

            statement.AppendLine(string.Format(cultureInfo, "Amount owed is {0:C}", totalAmount));
            statement.AppendLine(string.Format("You earned {0} credits", volumeCredits));

            return statement.ToString();
        }
    }
}
