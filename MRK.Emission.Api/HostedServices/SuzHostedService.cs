using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MRK.Emission.Business.Emission.Commands.ProceedOrderDocuments;
using MRK.Emission.Business.Emission.Commands.ProceedOrders;
using MRK.Emission.Service.SUZ;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MRK.Emission.Api.HostedServices
{
    public class SuzHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SuzServiceSettings _settings;
        private readonly ILogger<SuzHostedService> _logger;

        public SuzHostedService(IServiceProvider serviceProvider,
            IOptions<SuzServiceSettings> settings,
            ILogger<SuzHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _settings = settings.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                _logger.LogInformation($"ProceedOrderDocumentsCommand");

                try
                {
                    var command = new ProceedOrderDocumentsCommand();
                    var response = await mediator.Send(command, CancellationToken.None);

                    if (!response.Success)
                        _logger.LogError(response.ErrorMessage);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Couldn't process documents");
                }

                _logger.LogInformation($"ProceedOrdersCommand");

                try
                {
                    var command = new ProceedOrdersCommand();
                    var response = await mediator.Send(command, CancellationToken.None);

                    if (!response.Success)
                        _logger.LogError(response.ErrorMessage);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Couldn't process orders");
                }

                await Task.Delay(TimeSpan.FromMinutes(_settings.Interval), stoppingToken);
            }
        }
    }
}
