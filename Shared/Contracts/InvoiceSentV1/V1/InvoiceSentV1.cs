namespace Contracts.InvoiceSentV1.V1;

public class InvoiceSent
{
    public Guid DebtorId { get; init; }
    public Guid InvoiceId { get; set; }
}