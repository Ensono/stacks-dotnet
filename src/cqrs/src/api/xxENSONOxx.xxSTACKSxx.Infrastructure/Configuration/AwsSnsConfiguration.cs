using xxENSONOxx.xxSTACKSxx.Infrastructure.Secrets;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.Configuration;

public class AwsSnsConfiguration
{
    public Secret TopicArn { get; init; } = null!;
}
