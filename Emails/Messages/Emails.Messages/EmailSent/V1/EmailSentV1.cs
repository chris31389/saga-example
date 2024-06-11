namespace Emails.Messages.EmailSent.V1;

public class EmailSentV1
{
    public required Guid CorrelationId { get; init; }
    public required Guid EmailId { get; init; }
}