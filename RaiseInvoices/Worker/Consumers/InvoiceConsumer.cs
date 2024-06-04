using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace RaiseInvoice.Worker.Consumers;

public class InvoiceConsumer : IConsumer<RaiseInvoiceCommandV1>
{
    private readonly ILogger<InvoiceConsumer> _logger;

    public InvoiceConsumer(ILogger<InvoiceConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<RaiseInvoiceCommandV1> context)
    {
        _logger.LogInformation($"Consuming RaiseInvoiceCommand: {System.Text.Json.JsonSerializer.Serialize(context.Message)}");
        return Task.CompletedTask;
    }
}