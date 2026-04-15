using EmailService.Worker.Contracts;
using MassTransit;

namespace EmailService.Worker.DevSupport;

/// <summary>
/// Servicio de desarrollo que publica un mensaje de prueba en el bus
/// después del arranque para validar el pipeline completo.
/// Solo debe registrarse en el entorno Development.
/// </summary>
public sealed class DevTestPublisherService(
    IBus bus,
    ILogger<DevTestPublisherService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Espera a que MassTransit y el host estén completamente listos.
        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

        logger.LogWarning("=== [DEV] Publishing test email message ===");

        await bus.Publish(new SendEmailRequest(
            To: "destinatario@test.com",
            Subject: "Prueba local del EmailService",
            Body: "<h1>¡Hola!</h1><p>Este es un email de prueba enviado desde el Worker Service en modo Development.</p>",
            IsHtml: true
        ), stoppingToken);

        logger.LogWarning("=== [DEV] Test message published successfully. Check smtp4dev UI at http://localhost:5000 ===");
    }
}
