namespace JailTalk.Application.Contracts.Email;

public interface IEmailService
{
    /// <summary>
    /// Sends an email to the given email address.
    /// </summary>
    /// <param name="to">Recievers email address</param>
    /// <param name="subject">Email subject</param>
    /// <param name="body">Body of the email</param>
    /// <param name="isBodyHtml">To indicate that the body is a valid HTML content.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public void SendEmail(string to, string subject, string body, bool isBodyHtml = true, CancellationToken cancellationToken = default);
}
