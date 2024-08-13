using xxAMIDOxx.xxSTACKSxx.Shared.API.Configuration;
using xxAMIDOxx.xxSTACKSxx.Shared.API.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Serilog;
using Serilog.Sinks.InMemory;
using Serilog.Sinks.InMemory.Assertions;

namespace xxAMIDOxx.xxSTACKSxx.Shared.API.Tests.IntegrationTests.Middleware;

public class CorrelationIdMiddlewareTestSteps
{
    private readonly Dictionary<string, string> _configurationCollection;
    private const string CONFIGURATION_COLLECTION_SECTION_NAME = "CorrelationId";

    private readonly IHostBuilder _hostBuilder;
    private IHeaderDictionary _requestHeaders;
    private HttpContext _responseMessage;
    private InMemorySink _inMemoryLoggingSink;
    private bool _willSerilogEnrichLogsWithCorrelationId;


    public CorrelationIdMiddlewareTestSteps()
    {
        _configurationCollection = new Dictionary<string, string>();
        _hostBuilder = createHostBuilder();
        _requestHeaders = new HeaderDictionary();
        _inMemoryLoggingSink = new InMemorySink();
        _responseMessage = new DefaultHttpContext();
        _willSerilogEnrichLogsWithCorrelationId = false;
    }


    //
    //  Given
    //

    public void GivenTheConfigurationFileHasTheHeaderNameSetTo(string headerName)
    {
        _configurationCollection.Add($"{CONFIGURATION_COLLECTION_SECTION_NAME}:HeaderName", headerName);
    }


    public void GivenTheConfigurationFileDoesNotHaveTheHeaderNameSet()
    {
        _configurationCollection.Remove($"{CONFIGURATION_COLLECTION_SECTION_NAME}:HeaderName");
    }


    public void GivenTheConfigurationFileHasIncludeInResponseSetTo(bool includeInResponse)
    {
        _configurationCollection.Add($"{CONFIGURATION_COLLECTION_SECTION_NAME}:IncludeInResponse", includeInResponse.ToString());
    }


    public void GivenTheRequestHeaderHasAlreadyBeenSet(string headerName, string headerValue)
    {
        _requestHeaders = new HeaderDictionary(new Dictionary<string, StringValues>
        {
            { headerName, headerValue }
        });
    }


    public void GivenTheRequestHeaderHasNotAlreadyBeenSet()
    {
        _requestHeaders = new HeaderDictionary();
    }


    public void GivenSerilogIsConfiguredToEnrichLogsWithCorrelationId()
    {
        _willSerilogEnrichLogsWithCorrelationId = true;
        _hostBuilder.ConfigureServices(s => s.AddHttpContextAccessor());
    }


    //
    //  When
    //

    public async void WhenRequestIsSentToTheServer()
    {
        initializeInMemoryLogger();

        var host = await _hostBuilder.StartAsync();
        var testServer = host.GetTestServer();

        _responseMessage = await testServer.SendAsync(c =>
        {
            c.Request.Method = HttpMethods.Get;
            c.Request.Path = "/";
            foreach (var header in _requestHeaders)
            {
                c.Request.Headers.Add(header);
            }
        });

        _inMemoryLoggingSink = InMemorySink.Instance;
    }


    //
    //  Then
    //

    public void ThenRequestHeaderIsAddedWithTheName(string headerName)
    {
        _responseMessage.Request.Headers.ContainsKey(headerName).Should().BeTrue();
        _responseMessage.Request.Headers[headerName].ToString().Should().NotBeEmpty();
    }


    public void ThenRequestHeaderIsAddedWithTheNameAndValue(string headerName, string headerValue)
    {
        _responseMessage.Request.Headers.ContainsKey(headerName).Should().BeTrue();
        _responseMessage.Request.Headers[headerName].ToString().Should().Be(headerValue);
    }


    public void ThenRequestHeaderIsNotAddedWithTheName(string headerName)
    {
        _responseMessage.Request.Headers.ContainsKey(headerName).Should().BeFalse();
    }


    public void ThenResponseHeaderIsAddedWithTheName(string headerName)
    {
        _responseMessage.Response.Headers.ContainsKey(headerName).Should().BeTrue();
    }


    public void ThenResponseHeaderIsAddedWithTheNameAndValue(string headerName, string headerValue)
    {
        _responseMessage.Response.Headers.ContainsKey(headerName).Should().BeTrue();
        _responseMessage.Request.Headers[headerName].ToString().Should().Be(headerValue);
    }


    public void ThenResponseHeaderIsNotAddedWithTheName(string headerName)
    {
        _responseMessage.Response.Headers.ContainsKey(headerName).Should().BeFalse();
    }


    public void ThenLogsIncludePropertyWithTheName(string propertyName)
    {
        _inMemoryLoggingSink.Should().HaveMessage(LoggingMiddleware.MiddlewareStartingLogMessage)
                            .Appearing().Once()
                            .WithProperty(propertyName);

        _inMemoryLoggingSink.Should().HaveMessage(LoggingMiddleware.MiddlewareCompleteLogMessage)
                            .Appearing().Once()
                            .WithProperty(propertyName);
    }


    public void ThenLogsIncludePropertyWithTheNameAndValue(string headerName, string headerValue)
    {
        _inMemoryLoggingSink.Should().HaveMessage(LoggingMiddleware.MiddlewareStartingLogMessage)
                            .Appearing().Once()
                            .WithProperty(headerName)
                            .WithValue(headerValue);

        _inMemoryLoggingSink.Should().HaveMessage(LoggingMiddleware.MiddlewareCompleteLogMessage)
                            .Appearing().Once()
                            .WithProperty(headerName)
                            .WithValue(headerValue);
    }


    //
    //  Setup
    //

    private void initializeInMemoryLogger()
    {
        if (_willSerilogEnrichLogsWithCorrelationId)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.InMemory()
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId()
                .CreateLogger();
        }
        else
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.InMemory()
                .Enrich.FromLogContext()
                .CreateLogger();
        }
    }


    private IHostBuilder createHostBuilder()
    {
        return new HostBuilder().ConfigureWebHost(webBuilder =>
        {
            webBuilder.UseTestServer()
                      .Configure(app =>
                      {
                          app.UseMiddleware<CorrelationIdMiddleware>();
                          app.UseMiddleware<LoggingMiddleware>();
                      })
                      .ConfigureAppConfiguration((_, configBuilder) =>
                      {
                          configBuilder.AddInMemoryCollection(_configurationCollection);
                      })
                      .ConfigureLogging((_, builder) =>
                      {
                          builder.ClearProviders();
                          builder.AddSerilog();
                      })
                      .ConfigureServices((hostContext, services) =>
                      {
                          services.AddOptions();
                          services.AddLogging();
                          var correlationIdSection = hostContext.Configuration.GetSection(CONFIGURATION_COLLECTION_SECTION_NAME);
                          services.Configure<CorrelationIdConfiguration>(correlationIdSection);
                      });
        });
    }
}