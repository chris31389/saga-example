using System;
using System.Threading.Tasks;
using Invoices.Api.Correlations;
using Invoices.Messages.OrderCreated.V1;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Invoices.Api.Controllers;

[ApiController]
[Route("orders")]
public class OrderController(
    ILogger<OrderController> logger,
    ICorrelationContextAccessor correlationContextAccessor,
    IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpPost(Name = "CreateOrder")]
    public async Task<IActionResult> StartInvoiceCreation([FromBody] OrderRequestModel model)
    {
        // Create Unique Identifier that can track the order
        var orderId = Guid.NewGuid();

        // Start Order
        logger.LogInformation("Order started with identifier {Identifier}", orderId);
        // TODO: Persist an order

        await publishEndpoint.Publish(new OrderCreatedV1
        {
            CorrelationId = correlationContextAccessor.CorrelationContext.CorrelationId,
            CustomerId = model.CustomerId ?? throw new ArgumentNullException(nameof(model.CustomerId)),
            OrderId = orderId,
            Name = model.Name ?? throw new ArgumentNullException(nameof(model.Name)),
            Amount = model.Amount,
            Currency = model.Currency ?? throw new ArgumentNullException(nameof(model.Currency)),
            Email = model.Email ?? throw new ArgumentNullException(nameof(model.Email))
        });

        return Accepted();
    }
}