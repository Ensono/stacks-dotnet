using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Amido.Stacks.Testing.Settings
{
    public static class Configuration
    {
        private readonly static Lazy<IConfiguration> configuration = new Lazy<IConfiguration>(LoadConfiguration);

        public static IConfiguration Root => configuration.Value;

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

        public static T For<T>(string section = null)
        {
            if (string.IsNullOrEmpty(section))
                return configuration.Value.Get<T>();
            else
                return configuration.Value.GetSection(section).Get<T>();
        }

        public static IOptions<T> AsOption<T>(this T content) where T : class, new()
        {
            return Options.Create<T>(content);
        }
    }
}