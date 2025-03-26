using MassTransit;
using System.Net.Mime;
using TheatricalPlayersRefactoringKata.API.Requests;
using TheatricalPlayersRefactoringKata.Domain.Interfaces;
using TheatricalPlayersRefactoringKata.Domain.Models;
using TheatricalPlayersRefactoringKata.Domain.Services.Adapters;
using TheatricalPlayersRefactoringKata.Domain.Services;
using TheatricalPlayersRefactoringKata.Infrastructure.Repositories;
using TheatricalPlayersRefactoringKata.API.Extensions;
using TheatricalPlayersRefactoringKata.API.Enums;

namespace TheatricalPlayersRefactoringKata.API.Bus
{
    public class StatementRequestConsumer : IConsumer<StatementRequestMessage>
    {
        private readonly ILogger<StatementRequestMessage> _logger;

        public StatementRequestConsumer(ILogger<StatementRequestMessage> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StatementRequestMessage> context)
        {
            var repository = new Repository<StatementRequest>();

            var request = repository.Get(context.Message.Id)
                ?? throw new InvalidOperationException(string.Format("Statement Request with Id {0} was not found!", context.Message.Id));

            _logger.LogInformation("Received Statement Request with Id: {0}", context.Message.Id);

            request.Status = RequestStatusEnum.Processing;
            repository.Commit();

            await Task.Delay(10000);

            try
            {
                var format = request.Format.ToLower();

                (IStatementAdapter Adapter, string ContentType) values = format switch
                {
                    "txt" => (new StatementToTextAdapter(), MediaTypeNames.Text.Plain),
                    "xml" => (new StatementToXmlAdapter(), MediaTypeNames.Application.Xml),
                    _ => throw new Exception($"Invalid format: {request.Format}")
                };

                var result = await new StatementPrinter(values.Adapter).PrintAsync(request.Invoice, request.Plays);
                var fileName = string.Format("statement_file.{0}", format);
                var attachment = new AttachmentFile(fileName, result.ToFile(), values.ContentType);

                repository.Attach(request, attachment);
                request.Status = RequestStatusEnum.Completed;
            }
            catch (Exception ex)
            {
                request.Status = RequestStatusEnum.Failed;
                request.ErrorMessage = ex.Message;
            }

            repository.Commit();
        }
    }
}
