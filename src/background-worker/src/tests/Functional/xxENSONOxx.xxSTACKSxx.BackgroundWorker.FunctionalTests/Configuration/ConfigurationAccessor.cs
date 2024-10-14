using Microsoft.Extensions.Configuration;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.FunctionalTests.Configuration;

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
        var configurationRoot = GetIConfigurationRoot();
        configurationRoot.Bind(configuration);

        return configuration;
    }
}
