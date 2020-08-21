using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace xxAMIDOxx.xxSTACKSxx.API.Authentication
{
    public static class SwaggerGenOptionsExtensions
    {
        public static void ConfigureForJwtBearerAuthentication(
            this SwaggerGenOptions options,
            JwtBearerAuthenticationConfiguration jwtBearerAuthenticationConfiguration)
        {
            if (jwtBearerAuthenticationConfiguration.IsDisabled())
            {
                return;
            }

            options.AddSecurityDefinition(OpenApiSecurityDefinitions.OAuth2, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(jwtBearerAuthenticationConfiguration.OpenApiAuthorizationUrl)
                    }
                }
            });

            options.AddSecurityDefinition(OpenApiSecurityDefinitions.Bearer, new OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.OperationFilter<JwtBearerAuthenticationOperationFilter>();
        }
    }
}
