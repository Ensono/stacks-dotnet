using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Xunit;
using xxENSONOxx.xxSTACKSxx.Infrastructure.IntegrationTests.CosmosDb.DataModel;
using xxENSONOxx.xxSTACKSxx.Shared.Configuration;
using xxENSONOxx.xxSTACKSxx.Shared.Data.Documents.CosmosDB;
using Config = xxENSONOxx.xxSTACKSxx.Shared.Testing.Settings.Configuration;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.IntegrationTests.CosmosDb.Integration;

public class IntegrationFixture : IAsyncLifetime
{
    private CosmosClient CosmosClient;
    private CosmosDbConfiguration Settings;
    private SecretResolver _secretResolver;

    public async Task InitializeAsync()
    {
        Settings = Config.For<CosmosDbConfiguration>("CosmosDB");
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

        var cosmosDbKey = await _secretResolver.ResolveSecretAsync(Settings.SecurityKeySecret);
        CosmosClient = new CosmosClient(Settings.DatabaseAccountUri, cosmosDbKey);
        await CosmosClient.CreateDatabaseIfNotExistsAsync(Settings.DatabaseName);
        var database = CosmosClient.GetDatabase(Settings.DatabaseName);
        await database.CreateContainerIfNotExistsAsync(nameof(SampleEntity), $"/{nameof(SampleEntity.OwnerId)}");
    }

    public Task DisposeAsync()
    {
        // future optional: drop or clean database
        return Task.CompletedTask;
    }
}
