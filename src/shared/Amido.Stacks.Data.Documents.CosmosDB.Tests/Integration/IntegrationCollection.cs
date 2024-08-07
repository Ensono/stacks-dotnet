using Xunit;

namespace Amido.Stacks.Data.Documents.CosmosDB.Tests.Integration
{
    [CollectionDefinition(Name)]
    public class IntegrationCollection : ICollectionFixture<IntegrationFixture>
    {
        // Must be unique compared with other collections within the project
        public const string Name = "IntegrationFixture";
    }
}