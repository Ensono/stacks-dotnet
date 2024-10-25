namespace xxENSONOxx.xxSTACKSxx.Worker.FunctionalTests.Models.CloudEvents;

public interface ICloudEvent
{
    string Id { get; }
    string Subject { get; }
    DateTime? Time { get; }
}
