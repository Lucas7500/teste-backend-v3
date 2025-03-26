using Microsoft.AspNetCore.Mvc;
using TheatricalPlayersRefactoringKata.API.Requests;
using TheatricalPlayersRefactoringKata.Domain.Interfaces;
using TheatricalPlayersRefactoringKata.Domain.Services;
using TheatricalPlayersRefactoringKata.Domain.Services.Adapters;

namespace TheatricalPlayersRefactoringKata.API.Controllers
{
    public static class ApiEndpoints
    {
        public static void AddStatementEndpoints(this WebApplication app)
        {
            app.MapPost("/api/statement", ([FromBody] StatementRequest request) =>
            {
                (IStatementAdapter Adapter, string ContentType) values = request.Format.ToLower() switch
                {
                    "text" => (new StatementToTextAdapter(), "text/plain"),
                    "xml" => (new StatementToXmlAdapter(), "application/xml"),
                    _ => throw new Exception($"Invalid format: {request.Format}")
                };

                var result = new StatementPrinter(values.Adapter).Print(request.Invoice, request.Plays);

                return Results.Content(result, values.ContentType);
            })
            .WithOpenApi()
            .Produces<string>(StatusCodes.Status200OK);
        }
    }
}
