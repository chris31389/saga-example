namespace Emails.Messages.SendEmailCommand.V1;

public record SendEmailCommandV1
{
    public string ContentFromUrl { get; set; }
    public string EmailAddress { get; set; }
    public Guid CorrelationId { get; set; }
}