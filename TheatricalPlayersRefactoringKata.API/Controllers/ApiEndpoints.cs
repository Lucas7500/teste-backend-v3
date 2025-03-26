using MassTransit;
using Microsoft.AspNetCore.Mvc;
using TheatricalPlayersRefactoringKata.API.Bus;
using TheatricalPlayersRefactoringKata.API.Enums;
using TheatricalPlayersRefactoringKata.API.Requests;
using TheatricalPlayersRefactoringKata.Infrastructure.Repositories;

namespace TheatricalPlayersRefactoringKata.API.Controllers
{
    public static class ApiEndpoints
    {
        public static void AddStatementEndpoints(this WebApplication app)
        {
            app.MapGet("/api/statement-request", ([FromQuery] string id) =>
            {
                var repository = new Repository<StatementRequest>();

                var statementRequest = repository.Get(id);

                if (statementRequest == null)
                {
                    return Results.NotFound(string.Format("Statement Request Was Not Found With Id: {0}!", id));
                }

                if (statementRequest.Status == RequestStatusEnum.Completed)
                {
                    var attachment = repository.GetAttachmentFor(statementRequest);

                    return attachment == null
                        ? Results.NotFound(string.Format("Statement File Was Not Found for Statement Request With Id: {0}!", id))
                        : Results.File(attachment.Stream, attachment.ContentType);
                }

                return statementRequest.Status switch
                {
                    RequestStatusEnum.Pending => Results.Ok(string.Format("Statement Request With Id: {0} is Still Pending!", id)),
                    RequestStatusEnum.Processing => Results.Ok(string.Format("Statement Request With Id: {0} is Being Processed!", id)),
                    RequestStatusEnum.Failed => Results.Ok(string.Format("Statement Request With Id: {0} Failed with Message: '{1}'", id, statementRequest.ErrorMessage)),
                    _ => Results.BadRequest(string.Format("Statement Request With Id: {0} is in an Invalid State!", id))
                };
            });

            app.MapPost("/api/statement-request", async ([FromBody] StatementRequest request, [FromServices] IBus bus) =>
            {
                var repository = new Repository<StatementRequest>();

                repository.Add(request);
                repository.Commit();

                await bus.Publish(new StatementRequestMessage(request.Id));

                return Results.Ok(string.Format("Statement Requested with Success! Check for Id: {0}", request.Id));
            });
        }
    }
}
