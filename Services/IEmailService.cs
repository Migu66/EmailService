using EmailService.Worker.Contracts;

namespace EmailService.Worker.Services;

public interface IEmailService
{
    Task SendAsync(SendEmailRequest request, CancellationToken cancellationToken = default);
}
