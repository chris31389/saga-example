namespace Contracts.RaiseInvoiceCompleted.V1;

public class RaiseInvoiceCompletedV1
{
    public Guid DebtorId { get; init; }
    public Guid InvoiceId { get; set; }
    public Guid OrderId { get; set; }
    public string? Url { get; set; }
}