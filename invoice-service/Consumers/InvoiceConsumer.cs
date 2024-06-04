using System.Threading.Tasks;
using Contracts.RaiseInvoiceCommand.V1;
using MassTransit;

namespace InvoiceService.Consumers;

public class InvoiceConsumer : IConsumer<RaiseInvoiceCommandV1>
{
    public Task Consume(ConsumeContext<RaiseInvoiceCommandV1> context)
    {
        return Task.CompletedTask;
    }
}