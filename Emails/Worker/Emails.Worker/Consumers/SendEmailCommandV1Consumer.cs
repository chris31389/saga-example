using System;
using System.Threading.Tasks;
using Emails.Messages.EmailSent.V1;
using Emails.Messages.SendEmailCommand.V1;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Emails.Worker.Consumers;

public class SendEmailCommandV1Consumer(ILogger<SendEmailCommandV1Consumer> logger) : IConsumer<SendEmailCommandV1>
{
    public async Task Consume(ConsumeContext<SendEmailCommandV1> context)
    {
        logger.LogInformation("Consuming {EventName}: {CorrelationId}", 
            nameof(SendEmailCommandV1),
            context.Message.CorrelationId);

        var emailId = Guid.NewGuid();
        await Task.Delay(3000);

        logger.LogInformation("Publishing {EventName}: {CorrelationId}", 
            nameof(EmailSentV1),
            context.Message.CorrelationId);

        await context.Publish(new EmailSentV1
        {
            CorrelationId = context.Message.CorrelationId,
            EmailId = emailId
        });
    }
}