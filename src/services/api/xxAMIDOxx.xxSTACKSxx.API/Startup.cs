using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using xxAMIDOxx.xxSTACKSxx.API.Filters;
using System.Reflection;

namespace xxAMIDOxx.xxSTACKSxx.API
{
    public class Startup
    {
        // public Startup(IConfiguration configuration)
        // {
        //     Configuration = configuration;
        // }


        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            _hostingEnv = env;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly IHostingEnvironment _hostingEnv;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ValuesOptions>(Configuration.GetSection("Values"));

            services
                //.AddMvc()
                .AddMvcCore(
                //c =>
                //c.Conventions.Add(new ApiExplorerGroupPerVersionConvention())
                )
                .AddApiExplorer()
                .AddAuthorization()
                .AddFormatterMappings()
                .AddJsonFormatters(options => options.ContractResolver = new CamelCasePropertyNamesContractResolver())
                .AddCors()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opts =>
                    {
                        opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        opts.SerializerSettings.Converters.Add(new StringEnumConverter
                        {
                            CamelCaseText = true
                        });
                        //opts.SerializerSettings.Converters.Add(new StringEnumConverter(typeof(CamelCaseNamingStrategy)));
                    });

            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("all", new Info
                    {
                        Version = "all",
                        Title = "Swagger Petstore",
                        Description = "Swagger Petstore (ASP.NET Core 2.0)",
                        Contact = new Contact()
                        {
                            Name = "Swagger Codegen Contributors",
                            Url = "https://github.com/swagger-api/swagger-codegen",
                            Email = "apiteam@swagger.io"
                        },
                        TermsOfService = "http://swagger.io/terms/"
                    });

                    c.CustomSchemaIds(type => type.FriendlyId(false));
                    c.DescribeAllEnumsAsStrings();
                    c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");
                    
                    // Sets the basePath property in the Swagger document generated
                    //c.DocumentFilter<BasePathFilter>("/v1");

                    //Set default tags, shows on top, non defined tags appears at bottom
                    c.DocumentFilter<SwaggerDocumentTagger>(new Tag[] {
                            new Tag { Name = "Menu" },
                            new Tag { Name = "Category" },
                            new Tag { Name = "Item" }
                        }, new string[] { });

                    // Include DataAnnotation attributes on Controller Action parameters as Swagger validation rules (e.g required, pattern, ..)
                    // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                    c.OperationFilter<GeneratePathParamsValidationFilter>();
                    //c.OperationFilter<TagByApiExplorerSettingsOperationFilter>();

                    c.TagActionsBy((api) => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });

                    c.DocInclusionPredicate((docName, apiDesc) => { return true; });
                })
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info
                    {
                        Version = "v1",
                        Title = "Swagger Petstore",
                        Description = "Swagger Petstore (ASP.NET Core 2.0)",
                        Contact = new Contact()
                        {
                            Name = "Swagger Codegen Contributors",
                            Url = "https://github.com/swagger-api/swagger-codegen",
                            Email = "apiteam@swagger.io"
                        },
                        TermsOfService = "http://swagger.io/terms/"
                    });
                    c.CustomSchemaIds(type => type.FriendlyId(false));
                    c.DescribeAllEnumsAsStrings();
                    c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");

                    // Show only operations where route starts with
                    c.DocumentFilter<VersionPathFilter>("/v1");

                    // Include DataAnnotation attributes on Controller Action parameters as Swagger validation rules (e.g required, pattern, ..)
                    // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                    c.OperationFilter<GeneratePathParamsValidationFilter>();
                })
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v2", new Info
                    {
                        Version = "v2",
                        Title = "Swagger Petstore",
                        Description = "Swagger Petstore (ASP.NET Core 2.0)",
                        Contact = new Contact()
                        {
                            Name = "Swagger Codegen Contributors",
                            Url = "https://github.com/swagger-api/swagger-codegen",
                            Email = "apiteam@swagger.io"
                        },
                        TermsOfService = "http://swagger.io/terms/"
                    });
                    c.CustomSchemaIds(type => type.FriendlyId(false));
                    c.DescribeAllEnumsAsStrings();
                    c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");

                    // Sets the basePath property in the Swagger document generated
                    c.DocumentFilter<VersionPathFilter>("/v2");

                    // Include DataAnnotation attributes on Controller Action parameters as Swagger validation rules (e.g required, pattern, ..)
                    // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                    c.OperationFilter<GeneratePathParamsValidationFilter>();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                //use root path to serve swagger
                //c.RoutePrefix = string.Empty;
                c.DisplayOperationId();

                //TODO: Either use the SwaggerGen generated Swagger contract (generated from C# classes)
                c.SwaggerEndpoint("all/swagger.json", "Menu (all)");
                c.SwaggerEndpoint("v1/swagger.json", "Menu (version 1)");
                c.SwaggerEndpoint("v2/swagger.json", "Menu (version 2)");

                //TODO: Or alternatively use the original Swagger contract that's included in the static files
                // c.SwaggerEndpoint("/swagger-original.json", "Swagger Petstore Original");
            });
        }
    }
}
