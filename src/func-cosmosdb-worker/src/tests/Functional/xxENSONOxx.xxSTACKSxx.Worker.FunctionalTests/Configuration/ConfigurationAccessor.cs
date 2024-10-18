using Microsoft.Extensions.Configuration;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Configuration;

public static class ConfigurationAccessor
{
    private static IConfigurationRoot? _root;

    private static IConfigurationRoot GetIConfigurationRoot()
    {
        return _root ?? (_root = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build());
    }

    public static ConfigurationModel GetApplicationConfiguration()
    {
        var configuration = new ConfigurationModel();
        var iConfig = GetIConfigurationRoot();
        iConfig.Bind(configuration);

        return configuration;
    }
}
