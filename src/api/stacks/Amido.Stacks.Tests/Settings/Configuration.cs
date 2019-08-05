using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Amido.Stacks.Tests.Settings
{
    public static class Configuration
    {

        private readonly static Lazy<IConfiguration> configuration = new Lazy<IConfiguration>(LoadConfiguration);

        /// <summary>
        /// Do not call directly. Initialization method used for lazy loading
        /// </summary>
        private static IConfiguration LoadConfiguration()
        {
            return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            //.AddEnvironmentVariables()
            .Build();
        }

        public static T For<T>(string section)
        {
            return configuration.Value.GetSection(section).Get<T>();
        }

        public static IOptions<T> AsOption<T>(this T content) where T : class, new()
        {
            return Options.Create<T>(content);
        }

    }
}
