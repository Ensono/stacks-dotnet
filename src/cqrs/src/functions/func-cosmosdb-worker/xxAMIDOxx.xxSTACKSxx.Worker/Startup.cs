using Amido.Stacks.Configuration.Extensions;
using Amido.Stacks.Messaging.Azure.ServiceBus.Configuration;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Core;
using xxAMIDOxx.xxSTACKSxx.Worker;
using xxAMIDOxx.xxSTACKSxx.Worker.Logging;

[assembly: FunctionsStartup(typeof(Startup))]
namespace xxAMIDOxx.xxSTACKSxx.Worker;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        RegisterDependentServices(builder);

        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }

    protected virtual void RegisterDependentServices(IFunctionsHostBuilder builder)
    {
        var configuration = LoadConfiguration(builder);

        builder.Services
            .Configure<ChangeFeedListener>(configuration.GetSection(nameof(ChangeFeedListener)))
            .AddLogging(l => { l.AddSerilog(CreateLogger(configuration)); })
            .AddTransient(typeof(ILogger<>), typeof(LogAdapter<>));

        builder.Services.AddSecrets();

        // Add service bus
        builder.Services
            .Configure<ServiceBusConfiguration>(configuration.GetSection("ServiceBusConfiguration"))
            .AddServiceBus();
    }

    private static IConfiguration LoadConfiguration(IFunctionsHostBuilder builder)
    {
        return new ConfigurationBuilder()
            .SetBasePath(builder.GetContext().ApplicationRootPath)
            .AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables()
            .Build();
    }

    private static Logger CreateLogger(IConfiguration config)
    {
        return new LoggerConfiguration()
            .ReadFrom
            .Configuration(config)
            .CreateLogger();
    }
}
