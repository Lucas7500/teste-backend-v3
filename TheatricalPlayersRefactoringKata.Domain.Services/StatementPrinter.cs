using System.Globalization;
using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Interfaces;
using TheatricalPlayersRefactoringKata.Domain.Models;

namespace TheatricalPlayersRefactoringKata.Domain.Services;

public class StatementPrinter(IStatementAdapter statementAdapter)
{
    public async Task<string> PrintAsync(Invoice invoice, Dictionary<string, Play> plays)
    {
        return await statementAdapter.PrintAsync(invoice, plays, new CultureInfo("en-US"));
    }
}
