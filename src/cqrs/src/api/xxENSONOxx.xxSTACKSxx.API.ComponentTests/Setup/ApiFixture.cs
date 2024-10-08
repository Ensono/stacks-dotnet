﻿using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace xxENSONOxx.xxSTACKSxx.API.ComponentTests.Setup;

/// <summary>
/// ApiFixture will handle the HttpClient creation for the test scenarios.
/// Each fixture talking to the API should inherit from this class
/// </summary>
/// <typeparam name="TEntrypoint">The entrypoint of the API project</typeparam>
public abstract class ApiFixture<TEntryPoint> where TEntryPoint : class
{
    private HttpClient HttpClient { get; }

    protected HttpResponseMessage LastResponse { get; set; } = null!;

    protected ApiFixture()
    {
        #if EventPublisherServiceBus
        Environment.SetEnvironmentVariable("SERVICEBUS_CONNECTIONSTRING", "Endpoint=sb://FakeServiceBusEnddpoint.net/;SharedAccessKeyName=FakeAccessKeyName;SharedAccessKey=FakeAccessKey");
        #endif
        
        WebApplicationFactory<TEntryPoint> webAppFactory = new WebApplicationFactory<TEntryPoint>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(RegisterDependencies);
                ConfigureWebHost(builder);
            });
        
        HttpClient = webAppFactory.CreateClient();
    }

    /// <summary>
    /// Send the request and set the LastResponse
    /// </summary>
    /// <param name="method">Http method used in the request</param>
    /// <param name="url">relative url for API resource</param>
    /// <returns></returns>
    protected async Task SendAsync(HttpMethod method, string url)
    {
        await SendAsync(method, url, (string)null!);
    }

    /// <summary>
    /// Send the request and set the LastResponse
    /// </summary>
    /// <typeparam name="TBody">Type of bdy being sent</typeparam>
    /// <param name="method">Http method used in the request</param>
    /// <param name="url">relative url for API resource</param>
    /// <param name="body">body to be submitted to the API</param>
    /// <returns></returns>
    protected async Task SendAsync<TBody>(HttpMethod method, string url, TBody body)
    {
        lastResponseObject = null!;

        HttpRequestMessage msg = new HttpRequestMessage(method, url);

        if (!Equals(body, default(TBody)))
            msg.Content = CreateHttpContent(body);

        LastResponse = await HttpClient.SendAsync(msg);
    }


    /// <summary>
    /// Configure WebHost(if needed) before the API Startup is added to the pipeline
    /// </summary>
    protected virtual void ConfigureWebHost(IWebHostBuilder builder) { }

    /// <summary>
    /// Register the dependent services needed in the API
    /// </summary>
    protected abstract void RegisterDependencies(IServiceCollection collection);

    private static HttpContent CreateHttpContent<TContent>(TContent content)
    {
        if (EqualityComparer<TContent>.Default.Equals(content, default))
        {
            Console.WriteLine($"API Fixture serialized request of type {typeof(TContent).Name} as empty");
            return new ByteArrayContent([]);
        }

        var json = JsonConvert.SerializeObject(content);
        Console.WriteLine($"API Fixture serialized request of type {typeof(TContent).Name} into {json}");
        var result = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
        result.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return result;
    }

    object? lastResponseObject;

    internal async Task<TBody> GetResponseObject<TBody>()
    {
        lastResponseObject ??= await LastResponse.Content.ReadAsAsync<TBody>();

        return ((TBody)lastResponseObject!);
    }
}
