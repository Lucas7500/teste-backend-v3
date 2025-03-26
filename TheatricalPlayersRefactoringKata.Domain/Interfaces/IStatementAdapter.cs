using System.Globalization;
using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Models;

namespace TheatricalPlayersRefactoringKata.Domain.Interfaces
{
    public interface IStatementAdapter
    {
        string Print(Invoice invoice, Dictionary<string, Play> plays, CultureInfo cultureInfo);
    }
}
