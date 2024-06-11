using System;
using System.Text.Json;
using System.Threading.Tasks;
using Emails.Messages.EmailSent.V1;
using Emails.Messages.SendEmailCommand.V1;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Emails.Worker.Consumers;

public class SendEmailCommandV1Consumer : IConsumer<SendEmailCommandV1>
{
    private readonly ILogger<SendEmailCommandV1Consumer> _logger;

    public SendEmailCommandV1Consumer(ILogger<SendEmailCommandV1Consumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<SendEmailCommandV1> context)
    {
        var json = JsonSerializer.Serialize(context.Message);
        _logger.LogInformation("Consuming {Command}: {Json}", nameof(SendEmailCommandV1), json);

        var emailId = Guid.NewGuid();

        await context.Publish(new EmailSentV1
        {
            CorrelationId = context.Message.CorrelationId,
            EmailId = emailId
        });
    }
}