using System.IO;
using Microsoft.Extensions.Configuration;

namespace xxAMIDOxx.xxSTACKSxx.Provider.PactTests
{
    public class ConfigurationAccessor
    {
        static IConfigurationRoot root;

        private static IConfigurationRoot GetIConfigurationRoot()
        {
            if (root == null)
            {
                root = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();
            }

            return root;

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
