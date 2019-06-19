using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Amido.Stacks.E2e.Tests.Api
{
    public class Configuration
    {
        private static IConfigurationRoot GetIConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static AppConfig GetApplicationConfiguration()
        {
            var configuration = new AppConfig();

            var iConfig = GetIConfigurationRoot();

            iConfig.Bind(configuration);

            return configuration;
        }
    }
}
