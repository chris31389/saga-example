namespace Invoices.Messages.OrderStartedV1.V1;

public class OrderStartedV1
{
    public string? Name { get; set; }
    public string? Currency { get; set; }
    public decimal Amount { get; set; }
    public string? CustomerId { get; set; }
    public Guid OrderId { get; set; }
    public string Email { get; set; }
}