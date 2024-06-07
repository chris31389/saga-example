using System.Text.Json;
using System.Threading.Tasks;
using Contracts.CreateOrUpdateDebtorCommand.V1;
using Contracts.RaiseInvoiceCommand.V1;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Debtors.Worker.Consumers;

public class CreateOrUpdateDebtorCommandConsumer : IConsumer<CreateOrUpdateDebtorCommandV1>
{
    private readonly ILogger<CreateOrUpdateDebtorCommandConsumer> _logger;

    public CreateOrUpdateDebtorCommandConsumer(ILogger<CreateOrUpdateDebtorCommandConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<CreateOrUpdateDebtorCommandV1> context)
    {
        var json = JsonSerializer.Serialize(context.Message);
        _logger.LogInformation("Consuming {Command}: {Json}", nameof(RaiseInvoiceCommandV1), json);
        return Task.CompletedTask;
    }
}