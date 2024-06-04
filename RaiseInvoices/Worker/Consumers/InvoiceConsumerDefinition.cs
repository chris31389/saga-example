using MassTransit;

namespace RaiseInvoice.Worker.Consumers;

public class InvoiceConsumerDefinition : ConsumerDefinition<InvoiceConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<InvoiceConsumer> consumerConfigurator
    )
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}