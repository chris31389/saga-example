namespace Emails.Messages.EmailSent.V1;

public class EmailSentV1
{
    public Guid CorrelationId { get; init; }
    public Guid EmailId { get; init; }
}