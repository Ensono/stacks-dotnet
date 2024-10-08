using System;
using Microsoft.Extensions.Logging;

namespace xxENSONOxx.xxSTACKSxx.Listener.Logging;

/// <summary>
///     Allow use of ILogger<> at dependency registration level
///     Required to bypass issue with Azure Function DI not registering
///     logging dependencies soon enough
/// </summary>
public class LogAdapter<T>(ILoggerProvider provider) : ILogger<T>
{
    private readonly Lazy<ILogger> logger = new(() => provider.CreateLogger(typeof(T).Name));

    public IDisposable BeginScope<TState>(TState state) => logger.Value.BeginScope(state);

    public bool IsEnabled(LogLevel logLevel) => logger.Value.IsEnabled(logLevel);

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter
    ) => logger.Value.Log(logLevel, eventId, state, exception, formatter);
}
