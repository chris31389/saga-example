namespace Invoices.Api.Controllers;

public class OrderRequestModel
{
    public string? Name { get; set; }
    public string? Currency { get; set; }
    public decimal Amount { get; set; }
    public string CustomerId { get; set; }
    public string Email { get; set; }
}