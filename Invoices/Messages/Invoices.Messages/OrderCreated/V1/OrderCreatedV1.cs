namespace Invoices.Messages.OrderCreated.V1;

public class OrderCreatedV1
{
    public required string Name { get; init; }
    public required string Currency { get; init; }
    public required decimal Amount { get; init; }
    public required string CustomerId { get; init; }
    public required Guid OrderId { get; init; }
    public required string Email { get; init; } 
    public required Guid CorrelationId { get; init; }
}