using System;
using System.Threading.Tasks;
using Invoices.Messages.OrderStartedV1.V1;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Invoices.Api.Controllers;

[ApiController]
[Route("orders")]
public class OrderController(
    ILogger<OrderController> logger,
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

        await publishEndpoint.Publish(new OrderStartedV1
        {
            CustomerId = model.CustomerId,
            OrderId = orderId,
            Name = model.Name,
            Amount = model.Amount,
            Currency = model.Currency
        });

        return Accepted();
    }
}