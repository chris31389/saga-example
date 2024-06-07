namespace Contracts.RaiseInvoiceCommand.V1;

public record RaiseInvoiceCommandV1
{
    public string? DebtorId { get; set; }
    public decimal Value { get; set; }
    public string? Currency { get; set; }
}