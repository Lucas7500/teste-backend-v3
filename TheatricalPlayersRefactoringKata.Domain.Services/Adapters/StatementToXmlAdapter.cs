using System.Globalization;
using System.Xml.Serialization;
using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Interfaces;
using TheatricalPlayersRefactoringKata.Domain.Models;
using TheatricalPlayersRefactoringKata.Domain.Services.Factories;

namespace TheatricalPlayersRefactoringKata.Domain.Services.Adapters
{
    public class StatementToXmlAdapter : IStatementAdapter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays, CultureInfo cultureInfo)
        {
            var statement = new StatementXml
            {
                Customer = invoice.Customer,
                Items = []
            };

            var totalAmount = decimal.Zero;
            var totalCredits = 0;

            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayId];
                var lines = play.Lines;
                decimal thisAmount;
                int earnedCredits;

                var genre = GenreFactory.Create(play.Type, perf.Audience, play.Lines);

                thisAmount = genre.CalculateAmount();
                earnedCredits = genre.CalculateCredit();

                statement.Items.Add(new StatementItemXml
                {
                    AmountOwed = thisAmount,
                    EarnedCredits = earnedCredits,
                    Seats = perf.Audience
                });

                totalAmount += thisAmount;
                totalCredits += earnedCredits;
            }

            statement.AmountOwed = totalAmount;
            statement.EarnedCredits = totalCredits;

            return SerializeToXml(statement);
        }

        private static string SerializeToXml(StatementXml statement)
        {
            var xmlSerializer = new XmlSerializer(typeof(StatementXml));
            using var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, statement);
            return stringWriter.ToString();
        }
    }

    [XmlRoot("Statement")]
    public class StatementXml
    {
        public string Customer { get; set; }
        
        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<StatementItemXml> Items { get; set; }

        public decimal AmountOwed { get; set; }
        public int EarnedCredits { get; set; }
    }

    public class StatementItemXml
    {
        public decimal AmountOwed { get; set; }
        public int EarnedCredits { get; set; }
        public int Seats { get; set; }
    }
}
