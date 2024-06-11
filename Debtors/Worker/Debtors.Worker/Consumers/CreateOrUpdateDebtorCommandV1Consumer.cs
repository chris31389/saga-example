using System;
using System.Threading.Tasks;
using Debtors.Messages.CreateOrUpdateDebtorCommand.V1;
using Debtors.Messages.CreateOrUpdateDebtorCompleted.V1;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Debtors.Worker.Consumers;

public class CreateOrUpdateDebtorCommandV1Consumer : IConsumer<CreateOrUpdateDebtorCommandV1>
{
    private readonly ILogger<CreateOrUpdateDebtorCommandV1Consumer> _logger;

    public CreateOrUpdateDebtorCommandV1Consumer(ILogger<CreateOrUpdateDebtorCommandV1Consumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CreateOrUpdateDebtorCommandV1> context)
    {
        // Create Or Update Debtor
        var debtorId = Guid.NewGuid();

        await context.Publish(new CreateOrUpdateDebtorCompletedV1
        {
            CorrelationId = context.Message.CorrelationId,
            DebtorId = debtorId,
            Email = context.Message.Email
        });
    }
}