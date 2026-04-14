using System.ComponentModel.DataAnnotations;

namespace EmailService.Worker.Options;

public sealed class ServiceBusOptions
{
    public const string SectionName = "ServiceBusOptions";

    [Required]
    public string ConnectionString { get; init; } = string.Empty;

    [Required]
    public string QueueName { get; init; } = string.Empty;
}
