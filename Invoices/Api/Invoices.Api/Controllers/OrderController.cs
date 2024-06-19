using System;
using System.Threading;
using System.Threading.Tasks;
using Invoices.Api.Correlations;
using Invoices.Messages.OrderCreated.V1;
using Invoices.Persistence;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Invoices.Api.Controllers;

[ApiController]
[Route("orders")]
public class OrderController(
    ILogger<OrderController> logger,
    ICorrelationContextAccessor correlationContextAccessor,
    IMongoDatabase mongoDatabase,
    IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpPost(Name = "CreateOrder")]
    public async Task<IActionResult> StartInvoiceCreation([FromBody] OrderRequestModel model, CancellationToken cancellationToken)
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
        }, cancellationToken);

        var getUrl = Url.Action("State", new { orderId = orderId });
        return Accepted(getUrl);
    }

    [HttpGet("{orderId}/state", Name = "OrderState")]
    public async Task<IActionResult> State(Guid orderId, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving State for order {OrderId}", orderId);
        var collection = mongoDatabase.GetCollection<OrderSagaData>(Constants.CollectionName);
        var filter = Builders<OrderSagaData>.Filter.Eq(r => r.OrderId, orderId);
        var orderSagaData = await collection
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return orderSagaData != null
            ? Ok(orderSagaData)
            : NotFound();
    }
}