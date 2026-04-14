using EmailService.Worker.Contracts;
using EmailService.Worker.Services;
using MassTransit;

namespace EmailService.Worker.Consumers;

public sealed class EmailRequestConsumer(
    IEmailService emailService,
    ILogger<EmailRequestConsumer> logger) : IConsumer<SendEmailRequest>
{
    public async Task Consume(ConsumeContext<SendEmailRequest> context)
    {
        logger.LogInformation(
            "Processing email request for {To}. MessageId: {MessageId}",
            context.Message.To,
            context.MessageId);

        try
        {
            await emailService.SendAsync(context.Message, context.CancellationToken);

            logger.LogInformation(
                "Email request processed successfully for {To}. MessageId: {MessageId}",
                context.Message.To,
                context.MessageId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "Failed to process email request for {To}. MessageId: {MessageId}",
                context.Message.To,
                context.MessageId);
            throw;
        }
    }
}