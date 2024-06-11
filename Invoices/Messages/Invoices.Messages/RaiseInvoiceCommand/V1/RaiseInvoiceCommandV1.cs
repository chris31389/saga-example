namespace Invoices.Messages.RaiseInvoiceCommand.V1;

public record RaiseInvoiceCommandV1
{
    public Guid DebtorId { get; init; }
    public decimal Value { get; init; }
    public string Currency { get; init; }
    public Guid CorrelationId { get; init; }
}