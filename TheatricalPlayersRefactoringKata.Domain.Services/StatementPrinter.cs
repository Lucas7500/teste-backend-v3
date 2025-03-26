using System.Globalization;
using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Interfaces;
using TheatricalPlayersRefactoringKata.Domain.Models;

namespace TheatricalPlayersRefactoringKata.Domain.Services;

public class StatementPrinter(IStatementAdapter statementAdapter)
{
    public string Print(Invoice invoice, Dictionary<string, Play> plays)
    {
        return statementAdapter.Print(invoice, plays, new CultureInfo("en-US"));
    }
}
