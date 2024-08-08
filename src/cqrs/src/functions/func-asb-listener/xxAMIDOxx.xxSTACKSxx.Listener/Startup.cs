using Amido.Stacks.Messaging.Azure.ServiceBus.Configuration;
using Amido.Stacks.Messaging.Azure.ServiceBus.Serializers;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Core;
using xxAMIDOxx.xxSTACKSxx.Listener;
using xxAMIDOxx.xxSTACKSxx.Listener.Logging;

[assembly: FunctionsStartup(typeof(Startup))]
namespace xxAMIDOxx.xxSTACKSxx.Listener;

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
            .Configure<StacksListener>(configuration.GetSection(nameof(StacksListener)))
            .AddLogging(l => { l.AddSerilog(CreateLogger(configuration)); })
            .AddTransient(typeof(ILogger<>), typeof(LogAdapter<>));
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
