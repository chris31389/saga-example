using MassTransit;

namespace RaiseInvoices.Worker.Consumers;

public class RaiseInvoiceCommandV1ConsumerDefinition : ConsumerDefinition<RaiseInvoiceCommandV1Consumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<RaiseInvoiceCommandV1Consumer> consumerConfigurator,
        IRegistrationContext context) => endpointConfigurator
        .UseMessageRetry(r => r.Intervals(500, 1000));

}