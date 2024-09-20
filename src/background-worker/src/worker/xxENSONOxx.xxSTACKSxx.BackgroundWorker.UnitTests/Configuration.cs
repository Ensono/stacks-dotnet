using Microsoft.Extensions.Configuration;

namespace xxENSONOxx.xxSTACKSxx.BackgroundWorker.UnitTests;

public static class Configuration
{
    private static readonly Lazy<IConfiguration> configuration = new(LoadConfiguration);

    /// <summary>
    /// Do not call directly. Initialization method used for lazy loading
    /// </summary>
    private static IConfiguration LoadConfiguration()
    {
        //Load .env file if one found in output directory
        DotEnv.Load();

        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddIniFile("secrets.ini", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }

    public static T For<T>(string section = null!)
    {
        return string.IsNullOrEmpty(section)
            ? configuration.Value.Get<T>()!
            : configuration.Value.GetSection(section).Get<T>()!;
    }
}
