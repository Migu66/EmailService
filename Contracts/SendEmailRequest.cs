namespace EmailService.Worker.Contracts;

public record SendEmailRequest(
    string To,
    string Subject,
    string Body,
    bool IsHtml = false,
    string? Cc = null
);