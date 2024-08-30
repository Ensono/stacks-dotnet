using Amazon.SimpleNotificationService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using xxENSONOxx.xxSTACKSxx.Infrastructure.Publishers;
using xxENSONOxx.xxSTACKSxx.Shared.Application.CQRS.ApplicationEvents;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add the AWS SNS client for IEventConsumer and IApplicationEventPublisher
    /// </summary>
    public static IServiceCollection AddAwsSns(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonSimpleNotificationService>();
        services.AddTransient<IApplicationEventPublisher, EventPublisher>();

        return services;
    }
}
