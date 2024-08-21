using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using xxAMIDOxx.xxSTACKSxx.API.Authentication;
using xxAMIDOxx.xxSTACKSxx.API.Authorization;
using xxAMIDOxx.xxSTACKSxx.API.Models.Requests;
using xxAMIDOxx.xxSTACKSxx.Shared.API.Middleware;
using xxAMIDOxx.xxSTACKSxx.Shared.API.Swagger.Filters;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Swashbuckle.AspNetCore.SwaggerGen;
using xxAMIDOxx.xxSTACKSxx.API;
using xxAMIDOxx.xxSTACKSxx.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Host.UseSerilog((context, services, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container
var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

var pathBase = Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.ApiBasePathEnvName) ?? string.Empty;
var useAppInsights = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.AppInsightsEnvName));
var useOpenTelemetry = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.OtlpServiceName));

if (useAppInsights)
{
    services.AddApplicationInsightsTelemetry();
}

if (useOpenTelemetry)
{
    AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
    services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder.ConfigureResource(resource =>
                {
                    resource.AddService(Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.OtlpServiceName));
                });
                builder.AddAspNetCoreInstrumentation();
                builder.AddConsoleExporter(options =>
                {
                    options.Targets = ConsoleExporterOutputTargets.Debug;
                });
                builder.AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.OltpEndpoint));
                });
            });
}

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
    var projectUrl = "https://github.com/amido/stacks-dotnet-cqrs";

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
                    Name = "Amido",
                    Url = new Uri(projectUrl),
                    Email = "stacks@amido.com"
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
                    Name = "Amido",
                    Url = new Uri(projectUrl),
                    Email = "stacks@amido.com"
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
                    Name = "Amido",
                    Url = new Uri(projectUrl),
                    Email = "stacks@amido.com"
                }
            });

            c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{typeof(Program).Assembly.GetName().Name}.xml");
            c.DocumentFilter<VersionPathFilter>("/v2");
        });
}
