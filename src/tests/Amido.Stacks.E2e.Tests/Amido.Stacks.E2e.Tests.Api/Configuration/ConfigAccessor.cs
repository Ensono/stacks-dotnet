using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Amido.Stacks.E2e.Tests.Api.Configuration
{
    public class ConfigAccessor
    {
        private static IConfigurationRoot GetIConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public static ConfigModel GetApplicationConfiguration()
        {
            var configuration = new ConfigModel();

            var iConfig = GetIConfigurationRoot();

            iConfig.Bind(configuration);

            return configuration;
        }
    }
}
