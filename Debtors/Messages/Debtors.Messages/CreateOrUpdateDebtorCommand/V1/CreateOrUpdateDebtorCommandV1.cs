namespace Debtors.Messages.CreateOrUpdateDebtorCommand.V1;

public class CreateOrUpdateDebtorCommandV1
{
    public required string Name { get; init; }
    public required string CustomerId { get; init; }
    public required Guid CorrelationId { get; init; }
    public required string Email { get; init; }
}