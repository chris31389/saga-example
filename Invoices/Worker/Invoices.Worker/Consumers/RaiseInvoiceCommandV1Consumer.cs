using System.Text.Json;
using System.Threading.Tasks;
using Invoices.Messages.RaiseInvoiceCommand.V1;
using Invoices.Messages.RaiseInvoiceCompleted.V1;
using Invoices.Worker.CreateInvoices;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Invoices.Worker.Consumers;

public class RaiseInvoiceCommandV1Consumer(
    ICreateInvoiceService createInvoiceService,
    ILogger<RaiseInvoiceCommandV1Consumer> logger)
    : IConsumer<RaiseInvoiceCommandV1>
{
    public async Task Consume(ConsumeContext<RaiseInvoiceCommandV1> context)
    {
        var json = JsonSerializer.Serialize(context.Message);
        logger.LogInformation("Consuming {Command}: {Json}", nameof(RaiseInvoiceCommandV1), json);

        var invoiceId = await createInvoiceService.Create(context.Message.Currency, context.Message.Value);
        await context.Publish(new RaiseInvoiceCompletedV1
        {
            DebtorId = context.Message.DebtorId,
            CorrelationId = context.Message.CorrelationId,
            InvoiceId = invoiceId,
            Url = $"http://something.blob.com/{invoiceId}"
        });
    }
}