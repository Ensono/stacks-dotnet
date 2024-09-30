using Microsoft.Extensions.Configuration;

namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Configuration;

public static class ConfigAccessor
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

    public static ConfigModel GetApplicationConfiguration()
    {
        var configuration = new ConfigModel();

        var iConfig = GetIConfigurationRoot();

        iConfig.Bind(configuration);

        return configuration;
    }
}
