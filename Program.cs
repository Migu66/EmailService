using EmailService.Worker.Consumers;
using EmailService.Worker.Options;
using EmailService.Worker.Services;
using MassTransit;
using Microsoft.Extensions.Options;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog((services, config) =>
    config.ReadFrom.Configuration(builder.Configuration));

builder.Services
    .AddOptions<ServiceBusOptions>()
    .BindConfiguration(ServiceBusOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddOptions<SmtpOptions>()
    .BindConfiguration(SmtpOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddScoped<IEmailService, MailKitEmailService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<EmailRequestConsumer>();

    x.UsingAzureServiceBus((ctx, cfg) =>
    {
        var options = ctx.GetRequiredService<IOptions<ServiceBusOptions>>().Value;

        cfg.Host(options.ConnectionString);

        cfg.ReceiveEndpoint(options.QueueName, e =>
        {
            e.UseMessageRetry(r => r.Exponential(
                retryLimit: 3,
                minInterval: TimeSpan.FromSeconds(5),
                maxInterval: TimeSpan.FromSeconds(30),
                intervalDelta: TimeSpan.FromSeconds(5)));

            e.ConfigureConsumer<EmailRequestConsumer>(ctx);
        });
    });
});

var host = builder.Build();
host.Run();
