using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TheatricalPlayersRefactoringKata.Domain.Interfaces;
using TheatricalPlayersRefactoringKata.Domain.Models;
using TheatricalPlayersRefactoringKata.Domain.Services.Factories;

namespace TheatricalPlayersRefactoringKata.Domain.Services.Adapters
{
    public class StatementToXmlAdapter : IStatementAdapter
    {
        public async Task<string> PrintAsync(Invoice invoice, Dictionary<string, Play> plays, CultureInfo cultureInfo)
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

            return await Task.FromResult(SerializeToXml(statement));
        }

        private static string SerializeToXml(StatementXml statement)
        {
            var xmlSerializer = new XmlSerializer(typeof(StatementXml));

            var stringBuilder = new StringBuilder();

            var settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = false
            };

            using (var writer = XmlWriter.Create(stringBuilder, settings))
            {
                xmlSerializer.Serialize(writer, statement);
            }

            return stringBuilder.ToString();
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
