namespace Debtors.Messages.CreateOrUpdateDebtorCommand.V1;

public class CreateOrUpdateDebtorCommandV1
{
    public string Name { get; set; }
    public string CustomerId { get; set; }
    public Guid CorrelationId { get; set; }
    public string Email { get; set; }
}