using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace xxAMIDOxx.xxSTACKSxx.API.Authentication
{
    public class JwtBearerAuthenticationOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authorizeAttributes =
                context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                    .Union(context.MethodInfo.GetCustomAttributes(true))
                    .OfType<AuthorizeAttribute>();

            if (!authorizeAttributes.Any())
            {
                return;
            }

            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized. Access token is missing or invalid." });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden. The user does not have permission to execute this operation." });

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                {
                    new OpenApiSecurityRequirement
                    {
                        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = OpenApiSecurityDefinitions.OAuth2, Type = ReferenceType.SecurityScheme } }, new List<string>() },
                        { new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = OpenApiSecurityDefinitions.Bearer, Type = ReferenceType.SecurityScheme } }, new List<string>() },
                    }
                }
            };
        }
    }
}
