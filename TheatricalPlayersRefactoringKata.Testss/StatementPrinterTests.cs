using ApprovalTests;
using ApprovalTests.Reporters;
using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Models;
using TheatricalPlayersRefactoringKata.Domain.Services;
using TheatricalPlayersRefactoringKata.Domain.Services.Adapters;

namespace TheatricalPlayersRefactoringKata.Tests;

public class StatementPrinterTests
{
    [Fact]
    [UseReporter(typeof(DiffReporter))]
    public void TestTextStatementExample()
    {
        var plays = GetPlays();
        var invoice = GetInvoice();

        var adapter = new StatementToTextAdapter();
        var result = new StatementPrinter(adapter).Print(invoice, plays);

        Approvals.Verify(result);
    }
    
    [Fact]
    [UseReporter(typeof(DiffReporter))]
    public void TestXmlStatementExample()
    {
        var plays = GetPlays();
        var invoice = GetInvoice();

        var adapter = new StatementToXmlAdapter();
        var result = new StatementPrinter(adapter).Print(invoice, plays);

        Approvals.Verify(result);
    }

    private static Invoice GetInvoice()
    {
        return new(
            "BigCo",
            [
                new Performance("hamlet", 55),
                new Performance("as-like", 35),
                new Performance("othello", 40),
                new Performance("henry-v", 20),
                new Performance("john", 39),
                new Performance("henry-v", 20)
            ]
        );
    }

    private static Dictionary<string, Play> GetPlays()
    {
        return new Dictionary<string, Play>
        {
            { "hamlet", new Play("Hamlet", 4024, "tragedy") },
            { "as-like", new Play("As You Like It", 2670, "comedy") },
            { "othello", new Play("Othello", 3560, "tragedy") },
            { "henry-v", new Play("Henry V", 3227, "history") },
            { "john", new Play("King John", 2648, "history") },
            { "richard-iii", new Play("Richard III", 3718, "history") }
        };
    }
}
