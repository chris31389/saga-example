using System;
using System.Threading.Tasks;
using Debtors.Messages.CreateOrUpdateDebtorCommand.V1;
using Debtors.Messages.CreateOrUpdateDebtorCompleted.V1;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Debtors.Worker.Consumers;

public class CreateOrUpdateDebtorCommandV1Consumer(ILogger<CreateOrUpdateDebtorCommandV1Consumer> logger)
    : IConsumer<CreateOrUpdateDebtorCommandV1>
{
    public async Task Consume(ConsumeContext<CreateOrUpdateDebtorCommandV1> context)
    {
        logger.LogInformation("Consuming {EventName}: {CorrelationId}", 
            nameof(CreateOrUpdateDebtorCommandV1),
            context.Message.CorrelationId);

        // Create Or Update Debtor
        var debtorId = Guid.NewGuid();

        await Task.Delay(1000);
        logger.LogInformation("Publishing {EventName}: {CorrelationId}",
            nameof(CreateOrUpdateDebtorCompletedV1),
            context.Message.CorrelationId);

        await context.Publish(new CreateOrUpdateDebtorCompletedV1
        {
            CorrelationId = context.Message.CorrelationId,
            DebtorId = debtorId,
            Email = context.Message.Email
        });
    }
}