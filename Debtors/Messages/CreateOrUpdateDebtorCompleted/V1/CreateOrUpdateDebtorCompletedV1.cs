namespace Debtors.Messages.CreateOrUpdateDebtorCompleted.V1;

public class CreateOrUpdateDebtorCompletedV1
{
    public Guid DebtorId { get; init; }
    public Guid OrderId { get; set; }
    public string DebtorEmail { get; set; }
}