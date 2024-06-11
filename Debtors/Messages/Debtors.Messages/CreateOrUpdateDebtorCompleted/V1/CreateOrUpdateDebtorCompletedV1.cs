namespace Debtors.Messages.CreateOrUpdateDebtorCompleted.V1;

public class CreateOrUpdateDebtorCompletedV1
{
    public Guid DebtorId { get; set; }
    public Guid CorrelationId { get; set; }
}