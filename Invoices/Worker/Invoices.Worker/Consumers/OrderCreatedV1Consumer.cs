using System.Text.Json;
using System.Threading.Tasks;
using Invoices.Messages.OrderCreated.V1;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Invoices.Worker.Consumers;

/*public class OrderCreatedV1Consumer(ILogger<OrderCreatedV1> logger)
    : IConsumer<OrderCreatedV1>
{
    public async Task Consume(ConsumeContext<OrderCreatedV1> context)
    {
        var json = JsonSerializer.Serialize(context.Message);
        logger.LogInformation("Consuming {Command}: {Json}", nameof(OrderCreatedV1), json);
        await Task.CompletedTask;
    }
}*/