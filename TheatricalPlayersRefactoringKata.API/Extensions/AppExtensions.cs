using MassTransit;
using TheatricalPlayersRefactoringKata.API.Bus;

namespace TheatricalPlayersRefactoringKata.API.Extensions
{
    public static class AppExtensions
    {
        public static void AddRabbitMQService(this IServiceCollection services)
        {
            services.AddMassTransit(busConfig =>
            {
                busConfig.AddConsumer<StatementRequestConsumer>();

                busConfig.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri("amqp://localhost:5672"), host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });

                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }
    }
}
