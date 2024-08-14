using System;
using System.Threading.Tasks;
using xxAMIDOxx.xxSTACKSxx.Shared.Configuration;
using xxAMIDOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB.Tests.DataModel;
using xxAMIDOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB.Tests.Extensions;
using Microsoft.Azure.Cosmos;
using Xunit;
using config = xxAMIDOxx.xxSTACKSxx.Shared.Testing.Settings.Configuration;

namespace xxAMIDOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB.Tests.Integration
{
    public class IntegrationFixture : IAsyncLifetime
    {
        public CosmosClient CosmosClient;
        public CosmosDbConfiguration Settings;
        private SecretResolver _secretResolver;

        public async Task InitializeAsync()
        {
            Settings = config.For<CosmosDbConfiguration>("CosmosDB");
            _secretResolver = new SecretResolver();

            //Notes:
            // if using an azure instance to run the tests, make sure you set the environment variable before you start visual studio
            // Ex: CMD C:\> setx COSMOSDB_KEY ABCDEFGASD==
            // On CosmosDB, make sure you create the collection 'SampleEntity' in the database defined in the config 'Stacks'
            // To overrride the appsettings values, set the environment variable using SectionName__PropertyName. i.e: CosmosDB__DatabaseAccountUri
            // Note the use of a double _ between the section and the property name

            if (Environment.GetEnvironmentVariable(Settings.SecurityKeySecret.Identifier) == null)
            {
                //if localhost and running in visual studio
                if (Settings.DatabaseAccountUri.Contains("localhost", StringComparison.InvariantCultureIgnoreCase) &&
                    Environment.GetEnvironmentVariable("VisualStudioEdition") != null)
                {
                    Environment.SetEnvironmentVariable(Settings.SecurityKeySecret.Identifier,this.Settings.PrimaryKey);
                }
                else
                {
                    throw new ArgumentNullException(
                        $"The environment variable '{Settings.SecurityKeySecret.Identifier}' has not been set");
                }
            }

            string cosmosDbKey = await _secretResolver.ResolveSecretAsync(Settings.SecurityKeySecret);
            CosmosClient = new CosmosClient(Settings.DatabaseAccountUri, cosmosDbKey);
            await CosmosClient.CreateDatabaseIfNotExistsAsync(Settings.DatabaseName);
            Database database = CosmosClient.GetDatabase(Settings.DatabaseName);
            await database.CreateContainerIfNotExistsAsync(nameof(SampleEntity), $"/{nameof(SampleEntity.OwnerId)}");
        }

        public Task DisposeAsync()
        {
            // future optional: drop or clean database
            return Task.CompletedTask;
        }
    }
}