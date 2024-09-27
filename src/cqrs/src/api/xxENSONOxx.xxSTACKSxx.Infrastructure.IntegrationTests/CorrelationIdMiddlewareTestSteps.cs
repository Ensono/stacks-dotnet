using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using xxENSONOxx.xxSTACKSxx.API.Configuration;
using xxENSONOxx.xxSTACKSxx.API.Middleware;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.IntegrationTests;

public class CorrelationIdMiddlewareTestSteps
{
    private readonly Dictionary<string, string> _configurationCollection;
    private const string CONFIGURATION_COLLECTION_SECTION_NAME = "CorrelationId";

    private readonly IHostBuilder _hostBuilder;
    private IHeaderDictionary _requestHeaders;
    private HttpContext _responseMessage;

    public CorrelationIdMiddlewareTestSteps()
    {
        _configurationCollection = new Dictionary<string, string>();
        _hostBuilder = CreateHostBuilder();
        _requestHeaders = new HeaderDictionary();
        _responseMessage = new DefaultHttpContext();
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
        _hostBuilder.ConfigureServices(s => s.AddHttpContextAccessor());
    }
    
    //
    //  When
    //

    public async Task WhenRequestIsSentToTheServer()
    {
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

    //
    //  Setup
    //
    
    private IHostBuilder CreateHostBuilder()
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
                    configBuilder.AddInMemoryCollection(_configurationCollection!);
                })
                .ConfigureLogging((_, builder) =>
                {
                    builder.ClearProviders();
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
