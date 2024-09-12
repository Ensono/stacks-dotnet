#if CosmosDb
using Xunit;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.IntegrationTests.CosmosDb.Integration;

[CollectionDefinition(Name)]
public class IntegrationCollection : ICollectionFixture<IntegrationFixture>
{
    // Must be unique compared with other collections within the project
    public const string Name = "IntegrationFixture";
}
#endif
