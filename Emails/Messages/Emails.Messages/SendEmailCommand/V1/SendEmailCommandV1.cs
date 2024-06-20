namespace Emails.Messages.SendEmailCommand.V1;

public record SendEmailCommandV1
{
    public required string ContentFromUrl { get; init; }
    public required string EmailAddress { get; init; }
    public required Guid CorrelationId { get; init; }
}