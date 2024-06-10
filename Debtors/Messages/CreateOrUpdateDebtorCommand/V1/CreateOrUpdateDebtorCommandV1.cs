namespace Debtors.Messages.CreateOrUpdateDebtorCommand.V1;

public class CreateOrUpdateDebtorCommandV1
{
    public string? Name { get; set; }
    public string? CustomerId { get; init; }
    public Guid OrderId { get; set; }
    public string Email { get; set; }
}