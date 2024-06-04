using System.Threading.Tasks;
using InvoiceService.Contracts;
using MassTransit;

namespace InvoiceService.Consumers;

public class InvoiceConsumer :
    IConsumer<RaiseInvoiceCommand>
{
    public Task Consume(ConsumeContext<RaiseInvoiceCommand> context)
    {
        return Task.CompletedTask;
    }
}