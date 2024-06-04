namespace InvoiceService.Contracts;

public record RaiseInvoiceCommand
{
    public string DebtorId { get; set; }
    public decimal Value { get; set; }
    public string Currency { get; set; }
}