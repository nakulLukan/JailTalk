using JailTalk.Application.Contracts.Email;
using JailTalk.Shared.Utilities;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace JailTalk.Infrastructure.Impl.Email;

public class EmailService : IEmailService
{
    readonly ILogger<EmailService> _logger;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpAccountName;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly bool _emailServiceEnabled;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _emailServiceEnabled = bool.Parse(configuration["Email:EnableEmailService"]);

        string pathToSettings = "Email:SmtpSettings";
        _smtpServer = configuration[$"{pathToSettings}:Server"];
        _smtpPort = int.Parse(configuration[$"{pathToSettings}:Port"]);
        _smtpAccountName = configuration[$"{pathToSettings}:Name"];
        _smtpUsername = configuration[$"{pathToSettings}:UserName"];
        _smtpPassword = configuration[$"{pathToSettings}:Password"];
        _logger = logger;
    }

    /// <summary>
    /// Sends an email to the given email address.
    /// </summary>
    /// <param name="to">Recievers email address</param>
    /// <param name="subject">Email subject</param>
    /// <param name="body">Body of the email</param>
    /// <param name="isBodyHtml">To indicate that the body is a valid HTML content.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true, CancellationToken cancellationToken = default)
    {
        try
        {
            ThrowIfServiceIsNotEnabled();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpAccountName, _smtpUsername));
            message.To.Add(new MailboxAddress(string.Empty, to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            client.Connect(_smtpServer, _smtpPort, useSsl: false, cancellationToken);
            client.Authenticate(_smtpUsername, _smtpPassword);
            await client.SendAsync(message);
            client.Disconnect(true);
            _logger.LogInformation("Email sent successfully.");
        }
        catch (AppException)
        {
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email.");
        }
    }

    /// <summary>
    /// Use this function in all public method in the beginning to suppress any email activity.
    /// </summary>
    /// <returns></returns>
    private void ThrowIfServiceIsNotEnabled()
    {
        if (!_emailServiceEnabled)
        {
            _logger.LogWarning("Email service is not enabled. Bypassing any activities");
            throw new AppException("Email service is not emabled.");
        }
    }
}
