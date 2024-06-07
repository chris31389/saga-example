using MassTransit;

namespace Debtors.Worker.Consumers;

public class CreateOrUpdateDebtorCommandConsumerDefinition : ConsumerDefinition<CreateOrUpdateDebtorCommandConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<CreateOrUpdateDebtorCommandConsumer> consumerConfigurator,
        IRegistrationContext context) => endpointConfigurator
        .UseMessageRetry(r => r.Intervals(500, 1000));
}