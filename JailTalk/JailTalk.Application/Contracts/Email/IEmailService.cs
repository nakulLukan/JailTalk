namespace JailTalk.Application.Contracts.Email;

public interface IEmailService
{
    /// <summary>
    /// Function to generate body of the email from a given html template and its body params.
    /// Body params will get replaced in the template with actual value supplied.
    /// </summary>
    /// <param name="templateName"></param>
    /// <param name="bodyParams"></param>
    /// <returns></returns>
    public Task<string> GenerateBody(string templateName, IDictionary<string, string> bodyParams);

    /// <summary>
    /// Sends an email to the given email address.
    /// </summary>
    /// <param name="to">Recievers email address</param>
    /// <param name="subject">Email subject</param>
    /// <param name="body">Body of the email</param>
    /// <param name="isBodyHtml">To indicate that the body is a valid HTML content.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true, CancellationToken cancellationToken = default);
}
