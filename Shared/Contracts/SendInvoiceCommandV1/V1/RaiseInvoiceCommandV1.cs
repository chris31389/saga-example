namespace Contracts.SendInvoiceCommandV1.V1;

public record SendInvoiceCommandV1
{
    public Guid DebtorId { get; init; }
    public decimal Value { get; init; }
    public string? Currency { get; init; }
    public string Url { get; set; }
    public string Email { get; set; }
}