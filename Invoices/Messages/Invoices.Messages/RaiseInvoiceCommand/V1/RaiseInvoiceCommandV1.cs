namespace Invoices.Messages.RaiseInvoiceCommand.V1;

public record RaiseInvoiceCommandV1
{
    public required Guid DebtorId { get; init; }
    public required decimal Value { get; init; }
    public required string Currency { get; init; }
    public required Guid CorrelationId { get; init; }
}