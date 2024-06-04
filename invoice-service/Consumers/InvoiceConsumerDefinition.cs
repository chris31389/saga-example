using MassTransit;

namespace InvoiceService.Consumers;

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