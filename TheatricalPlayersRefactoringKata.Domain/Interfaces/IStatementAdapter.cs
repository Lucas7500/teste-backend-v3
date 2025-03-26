using System.Globalization;
using TheatricalPlayersRefactoringKata.Domain.Models;

namespace TheatricalPlayersRefactoringKata.Domain.Interfaces
{
    public interface IStatementAdapter
    {
        Task<string> PrintAsync(Invoice invoice, Dictionary<string, Play> plays, CultureInfo cultureInfo);
    }
}
