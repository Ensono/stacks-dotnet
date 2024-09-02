using xxENSONOxx.xxSTACKSxx.Shared.Configuration;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;

public class AwsSnsConfiguration
{
    public Secret TopicArn { get; init; } = null!;
}
