using Microsoft.Extensions.Logging;

namespace xxENSONOxx.xxSTACKSxx.Worker.UnitTests.Doubles;

public class MockLogger<T> : ILogger<T>
{
    public List<string> LogMessages { get; } = [];

    IDisposable ILogger.BeginScope<TState>(TState state) => null!;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        LogMessages.Add(formatter(state, exception));
    }
}

