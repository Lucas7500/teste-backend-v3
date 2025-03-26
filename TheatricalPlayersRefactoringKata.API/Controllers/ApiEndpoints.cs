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
            var repository = new Repository<StatementRequest>();

            app.MapGet("/api/statement-request", ([FromQuery] string id) =>
            {
                var attachment = repository.GetAttachmentFor(id);

                return attachment == null
                    ? Results.NotFound("Statement File Was Not Found!")
                    : Results.File(attachment.Stream, attachment.ContentType);
            });

            app.MapPost("/api/statement-request", ([FromBody] StatementRequest request) =>
            {
                (IStatementAdapter Adapter, string ContentType) values = request.Format.ToLower() switch
                {
                    "txt" => (new StatementToTextAdapter(), MediaTypeNames.Text.Plain),
                    "xml" => (new StatementToXmlAdapter(), MediaTypeNames.Application.Xml),
                    _ => throw new Exception($"Invalid format: {request.Format}")
                };

                var result = new StatementPrinter(values.Adapter).Print(request.Invoice, request.Plays);
                var fileName = request.Format.ToStatementFileName();
                var attachment = new AttachmentFile(fileName, result.ToFile(), values.ContentType);

                repository.Add(request);
                repository.Attach(request, attachment);
                repository.Commit();

                return Results.Ok(string.Format("Check for Statement Request with Id: {0}", request.Id));
            })
            .WithOpenApi()
            .Produces<string>(StatusCodes.Status200OK);
        }

        private static string ToStatementFileName(this string format)
        {
            return string.Format("statement_file.{0}", format.ToLower());
        }
    }
}
