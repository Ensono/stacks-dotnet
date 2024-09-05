﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using xxENSONOxx.xxSTACKSxx.Shared.API.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using xxENSONOxx.xxSTACKSxx.API;
using xxENSONOxx.xxSTACKSxx.API.Authentication;
using xxENSONOxx.xxSTACKSxx.API.Authorization;
using xxENSONOxx.xxSTACKSxx.API.Filters;
using xxENSONOxx.xxSTACKSxx.API.Models.Requests;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
var pathBase = Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.ApiBasePathEnvName) ?? string.Empty;
var useAppInsights = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.AppInsightsEnvName)!);
var useOpenTelemetry = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.OtlpServiceName)!);

builder.Host.UseSerilog(logger);

builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, ConfigurableAuthorizationPolicyProvider>();

if (useAppInsights)
{
    builder.Services.AddApplicationInsightsTelemetry();
}

if (useOpenTelemetry)
{
    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
    builder.Services.AddOpenTelemetry()
        .WithTracing(tracingBuilder =>
        {
            tracingBuilder.ConfigureResource(resource =>
            {
                resource.AddService(Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.OtlpServiceName)!);
            });
            tracingBuilder.AddAspNetCoreInstrumentation();
            tracingBuilder.AddConsoleExporter(options =>
            {
                options.Targets = ConsoleExporterOutputTargets.Debug;
            });
            tracingBuilder.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.OltpEndpoint)!);
            });
        });
}

var jwtBearerAuthenticationConfigurationSection = configuration.GetJwtBearerAuthenticationConfigurationSection();
builder.Services.Configure<JwtBearerAuthenticationConfiguration>(jwtBearerAuthenticationConfigurationSection);
var jwtBearerAuthenticationConfiguration = jwtBearerAuthenticationConfigurationSection.Get<JwtBearerAuthenticationConfiguration>();
builder.Services.AddJwtBearerTokenAuthentication(jwtBearerAuthenticationConfiguration);

builder.Services.AddMvcCore()
    .AddApiExplorer()
    .AddAuthorization()
    .AddDataAnnotations()
    .AddCors();

AddSwagger(builder.Services, jwtBearerAuthenticationConfiguration);

var app = builder.Build();

app.UseCorrelationId();
app.UsePathBase(pathBase);
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();
app.MapGet("/", context =>
{
    context.Response.Redirect((Environment.GetEnvironmentVariable("API_BASEPATH") ?? string.Empty) + "/swagger");
    return Task.CompletedTask;
});

app.UseSwagger(c =>
{
    c.PreSerializeFilters.Add((swagger, _) =>
    {
        swagger.Servers = new List<OpenApiServer> { new() { Url = $"{pathBase}" } };
    });
});

app.UseSwaggerUI(c =>
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

await app.RunAsync();

static void AddSwagger(IServiceCollection services, JwtBearerAuthenticationConfiguration jwtBearerAuthenticationConfiguration = null)
{
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("all", new OpenApiInfo
        {
            Version = "all",
            Title = "Menu API",
            Description = "APIs used to interact and manage menus",
            Contact = new OpenApiContact()
            {
                Name = "Ensono",
                Url = new Uri("https://github.com/ensono/stacks-dotnet"),
                Email = "stacks@ensono.com"
            },
        });

        c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{typeof(Program).Assembly.GetName().Name}.xml");
        c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{typeof(CreateMenuRequest).Assembly.GetName().Name}.xml");

        c.DocumentFilter<SwaggerDocumentTagger>(new OpenApiTag[] {
            new() { Name = "Menu" },
            new() { Name = "Category" },
            new() { Name = "Item" }
        }, Array.Empty<string>());

        c.TagActionsBy((api) => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });

        c.DocInclusionPredicate((docName, apiDesc) => { return true; });

        c.CustomOperationIds(apiDesc =>
        {
            return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
        });

        c.ConfigureForJwtBearerAuthentication(jwtBearerAuthenticationConfiguration);
    });

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Menu API",
            Description = "APIs used to interact and manage menus",
            Contact = new OpenApiContact()
            {
                Name = "Ensono",
                Url = new Uri("https://github.com/ensono/stacks-dotnet"),
                Email = "stacks@ensono.com"
            },
        });

        c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{typeof(Program).Assembly.GetName().Name}.xml");

        c.DocumentFilter<VersionPathFilter>("/v1");
    });

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v2", new OpenApiInfo
        {
            Version = "v2",
            Title = "Menu API",
            Description = "APIs used to interact and manage menus for a restaurant",
            Contact = new OpenApiContact()
            {
                Name = "Ensono",
                Url = new Uri("https://github.com/ensono/stacks-dotnet"),
                Email = "stacks@ensono.com"
            },
        });

        c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{typeof(Program).Assembly.GetName().Name}.xml");

        c.DocumentFilter<VersionPathFilter>("/v2");
    });
}
