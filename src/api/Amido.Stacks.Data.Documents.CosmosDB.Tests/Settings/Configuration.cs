using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Amido.Stacks.Data.Documents.CosmosDB.Tests.Settings
{
    public static class Configuration
    {
        private static IConfiguration LoadConfiguration()
        {
            return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            //.AddEnvironmentVariables()
            .Build();

        }

        public static T For<T>(string section)
        {
            var result = Activator.CreateInstance<T>();

            var config = LoadConfiguration().GetSection(section);

            config.Bind(result);

            return result;
        }

        public static IOptions<T> AsOption<T>(this T content) where T : class, new()
        {
            return Options.Create<T>(content);
        }

    }
}
