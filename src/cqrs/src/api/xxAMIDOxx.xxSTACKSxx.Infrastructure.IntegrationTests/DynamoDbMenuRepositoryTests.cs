using Xunit;

namespace xxAMIDOxx.xxSTACKSxx.Infrastructure.IntegrationTests;

/// <summary>
/// The purpose of this integration test is to validate the implementation
/// of MenuRepository againt the data store at development\integration
/// It is not intended to test if the configuration is valid for a release
/// Configuration issues will be surfaced on e2e or acceptance tests
/// </summary>
[Trait("TestType", "IntegrationTests")]
public class DynamoDbMenuRepositoryTests
{
    public DynamoDbMenuRepositoryTests()
    {

    }
}
