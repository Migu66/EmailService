using EmailService.Worker.Contracts;
using EmailService.Worker.Options;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;

namespace EmailService.Worker.Services;

public sealed class MailKitEmailService(
    IOptions<SmtpOptions> options,
    ILogger<MailKitEmailService> logger) : IEmailService
{
    private readonly SmtpOptions _smtp = options.Value;

    public async Task SendAsync(SendEmailRequest request, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Sending email to {To} with subject {Subject}", request.To, request.Subject);

        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtp.SenderName, _smtp.SenderEmail));
            message.To.Add(MailboxAddress.Parse(request.To));
            
            if (!string.IsNullOrWhiteSpace(request.Cc))
                message.Cc.Add(MailboxAddress.Parse(request.Cc));
            
            message.Subject = request.Subject;
            message.Body = request.IsHtml
                ? new BodyBuilder { HtmlBody = request.Body }.ToMessageBody()
                : new BodyBuilder { TextBody = request.Body }.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtp.Host, _smtp.Port, cancellationToken: cancellationToken);
            await client.AuthenticateAsync(_smtp.Username, _smtp.Password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);

            logger.LogInformation("Email successfully sent to {To}", request.To);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {To}", request.To);
            throw;
        }
    }
}
