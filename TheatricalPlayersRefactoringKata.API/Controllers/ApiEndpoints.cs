using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TheatricalPlayersRefactoringKata.API.Extensions;
using TheatricalPlayersRefactoringKata.API.Requests;
using TheatricalPlayersRefactoringKata.Domain.Interfaces;
using TheatricalPlayersRefactoringKata.Domain.Models;
using TheatricalPlayersRefactoringKata.Domain.Services;
using TheatricalPlayersRefactoringKata.Domain.Services.Adapters;
using TheatricalPlayersRefactoringKata.Infrastructure.Repositories;

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
                    "txt" => (new StatementToTextAdapter(), MediaTypeNames.Text.Plain),
                    "xml" => (new StatementToXmlAdapter(), MediaTypeNames.Application.Xml),
                    _ => throw new Exception($"Invalid format: {request.Format}")
                };

                var result = new StatementPrinter(values.Adapter).Print(request.Invoice, request.Plays);

                var repository = new Repository<StatementRequest>();
                var fileName = string.Concat("statement_file.", request.Format.ToLower());
                var attachment = new AttachmentFile(fileName, result.ToFile(), values.ContentType);

                repository.Add(request);
                repository.Attach(request, attachment);
                repository.Commit();

                return Results.Content(result, attachment.ContentType);
            })
            .WithOpenApi()
            .Produces<string>(StatusCodes.Status200OK);
        }
    }
}
