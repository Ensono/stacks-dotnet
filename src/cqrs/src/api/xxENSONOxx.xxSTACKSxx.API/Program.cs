using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using xxENSONOxx.xxSTACKSxx.API.Authentication;
using xxENSONOxx.xxSTACKSxx.API.Authorization;
using xxENSONOxx.xxSTACKSxx.API.Models.Requests;
using xxENSONOxx.xxSTACKSxx.Shared.API.Middleware;
using Microsoft.OpenApi.Models;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Swashbuckle.AspNetCore.SwaggerGen;
using xxENSONOxx.xxSTACKSxx.API;
using xxENSONOxx.xxSTACKSxx.API.Filters;
using xxENSONOxx.xxSTACKSxx.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

var pathBase = Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.ApiBasePathEnvName) ?? string.Empty;
var useAppInsights = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.AppInsightsEnvName));
var otlpServiceName = Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.OtlpServiceName) ?? "defaultServiceName";

if (useAppInsights)
{
    services.AddApplicationInsightsTelemetry();
}

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

// Register OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(tracingBuilder =>
    {
        tracingBuilder.ConfigureResource(resource =>
        {
            resource.AddService(otlpServiceName);
        });
        tracingBuilder.AddAspNetCoreInstrumentation();
        tracingBuilder.AddConsoleExporter(options =>
        {
            options.Targets = ConsoleExporterOutputTargets.Debug;
        });
    })
    .WithMetrics(metricProviderBuilder =>
    {
        metricProviderBuilder.ConfigureResource(resource =>
        {
            resource.AddService(otlpServiceName);
        });
        metricProviderBuilder.AddAspNetCoreInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter();
    })
    .WithLogging(loggerProviderBuilder =>
    {
        loggerProviderBuilder.ConfigureResource(resource =>
        {
            resource.AddService(otlpServiceName);
        });
        loggerProviderBuilder.AddConsoleExporter();
    })
    .UseOtlpExporter();

// Register OpenTelemetry with Azure Monitor
builder.Services.AddOpenTelemetry()
    .UseAzureMonitor();

services.AddHealthChecks();

services
    .AddMvcCore()
    .AddApiExplorer()
    .AddAuthorization()
    .AddDataAnnotations()
    .AddCors();

services.AddHttpContextAccessor();

var jwtBearerAuthenticationConfigurationSection = configuration.GetJwtBearerAuthenticationConfigurationSection();
services.Configure<JwtBearerAuthenticationConfiguration>(jwtBearerAuthenticationConfigurationSection);
var jwtBearerAuthenticationConfiguration = jwtBearerAuthenticationConfigurationSection.Get<JwtBearerAuthenticationConfiguration>();
services.AddJwtBearerTokenAuthentication(jwtBearerAuthenticationConfiguration);

services.AddSingleton<IAuthorizationPolicyProvider, ConfigurableAuthorizationPolicyProvider>();

AddSwagger(services, jwtBearerAuthenticationConfiguration);

DependencyRegistration.ConfigureStaticDependencies(services);
DependencyRegistration.ConfigureProductionDependencies(configuration, services);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCustomExceptionHandler(app.Logger);
app.UseCorrelationId();

app.UsePathBase(pathBase)
   .UseRouting()
   .UseAuthentication()
   .UseAuthorization()
   .UseEndpoints(endpoints =>
   {
       endpoints.MapHealthChecks("/health");
       endpoints.MapControllers();
       endpoints.MapGet("/", context =>
       {
           context.Response.Redirect((Environment.GetEnvironmentVariable("API_BASEPATH") ?? string.Empty) + "/swagger");
           return Task.CompletedTask;
       });
   })
   .UseSwagger(c =>
   {
       c.PreSerializeFilters.Add((swagger, httpReq) =>
       {
           swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{pathBase}" } };
       });
   })
   .UseSwaggerUI(c =>
   {
       c.DisplayOperationId();
       c.SwaggerEndpoint("all/swagger.json", "Menu (all)");
       c.SwaggerEndpoint("v1/swagger.json", "Menu (version 1)");
       c.SwaggerEndpoint("v2/swagger.json", "Menu (version 2)");

       if (jwtBearerAuthenticationConfiguration.HasOpenApiClient())
       {
           c.OAuthClientId(jwtBearerAuthenticationConfiguration.OpenApi.ClientId);
           c.OAuthUsePkce();
       }
   });

app.Run();

void AddSwagger(IServiceCollection services, JwtBearerAuthenticationConfiguration jwtBearerAuthenticationConfiguration = null)
{
    var projectUrl = "https://github.com/ensono/stacks-dotnet-cqrs";

    services
        .AddSwaggerGen(c =>
        {
            c.SwaggerDoc("all", new OpenApiInfo
            {
                Version = "all",
                Title = "Menu API",
                Description = "APIs used to interact and manage menus for a restaurant",
                Contact = new OpenApiContact
                {
                    Name = "Ensono",
                    Url = new Uri(projectUrl),
                    Email = "stacks@ensono.com"
                }
            });

            c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{typeof(Program).Assembly.GetName().Name}.xml");
            c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{typeof(CreateMenuRequest).Assembly.GetName().Name}.xml");

            c.DocumentFilter<SwaggerDocumentTagger>(new OpenApiTag[]
            {
                new OpenApiTag { Name = "Menu" },
                new OpenApiTag { Name = "Category" },
                new OpenApiTag { Name = "Item" }
            }, new string[] { });

            c.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
            c.DocInclusionPredicate((docName, apiDesc) => true);
            c.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : null);
            c.ConfigureForJwtBearerAuthentication(jwtBearerAuthenticationConfiguration);
        })
        .AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Menu API",
                Description = "APIs used to interact and manage menus for a restaurant",
                Contact = new OpenApiContact
                {
                    Name = "Ensono",
                    Url = new Uri(projectUrl),
                    Email = "stacks@ensono.com"
                }
            });

            c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{typeof(Program).Assembly.GetName().Name}.xml");
            c.DocumentFilter<VersionPathFilter>("/v1");
        })
        .AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v2", new OpenApiInfo
            {
                Version = "v2",
                Title = "Menu API",
                Description = "APIs used to interact and manage menus for a restaurant",
                Contact = new OpenApiContact
                {
                    Name = "Ensono",
                    Url = new Uri(projectUrl),
                    Email = "stacks@ensono.com"
                }
            });

            c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{typeof(Program).Assembly.GetName().Name}.xml");
            c.DocumentFilter<VersionPathFilter>("/v2");
        });
}
