namespace Invoices.Messages.RaiseInvoiceCompleted.V1;

public class RaiseInvoiceCompletedV1
{
    public required Guid DebtorId { get; init; }
    public required Guid InvoiceId { get; init; }
    public required Guid CorrelationId { get; init; }
    public required string Url { get; init; }
}