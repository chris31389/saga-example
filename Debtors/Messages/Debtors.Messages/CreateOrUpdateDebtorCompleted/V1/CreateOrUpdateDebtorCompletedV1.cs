namespace Debtors.Messages.CreateOrUpdateDebtorCompleted.V1;

public class CreateOrUpdateDebtorCompletedV1
{
    public required Guid DebtorId { get; init; }
    public required Guid CorrelationId { get; init; }
    public required string Email { get; init; }
}