using System;
using System.Text.Json;
using System.Threading.Tasks;
using Contracts.RaiseInvoiceCommand.V1;
using Contracts.RaiseInvoiceCompleted.V1;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace RaiseInvoices.Worker.Consumers;

public class RaiseInvoiceCommandV1Consumer : IConsumer<RaiseInvoiceCommandV1>
{
    private readonly ILogger<RaiseInvoiceCommandV1Consumer> _logger;

    public RaiseInvoiceCommandV1Consumer(ILogger<RaiseInvoiceCommandV1Consumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<RaiseInvoiceCommandV1> context)
    {
        var json = JsonSerializer.Serialize(context.Message);
        _logger.LogInformation("Consuming {Command}: {Json}", nameof(RaiseInvoiceCommandV1), json);

        var invoiceId = Guid.NewGuid();

        await context.Publish(new RaiseInvoiceCompletedV1
        {
            DebtorId = context.Message.DebtorId,
            InvoiceId = invoiceId
        });
    }
}