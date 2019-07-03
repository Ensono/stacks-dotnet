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
        public IConfiguration Configuration { get; }
        private readonly IHostingEnvironment _hostingEnv;
        private string pathBase = String.Empty;

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            _hostingEnv = env;
            Configuration = configuration;
            pathBase = Environment.GetEnvironmentVariable("API_BASEPATH") ?? String.Empty;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ValuesOptions>(Configuration.GetSection("Values"));

            services
                //.AddMvc()
                .AddMvcCore()
                .AddApiExplorer()
                .AddAuthorization()
                .AddFormatterMappings()
                .AddJsonFormatters(options => options.ContractResolver = new CamelCasePropertyNamesContractResolver())
                .AddCors()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opts =>
                    {
                        opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        opts.SerializerSettings.Converters.Add(new StringEnumConverter(typeof(CamelCaseNamingStrategy)));
                    });

            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("all", new Info
                    {
                        Version = "all",
                        Title = "Menu API",
                        Description = "APIs used to interact and manage menus for a restaurant",
                        Contact = new Contact()
                        {
                            Name = "Amido",
                            Url = "https://github.com/amido/stacks-dotnet",
                            Email = "stacks@amido.com"
                        },
                        TermsOfService = "http://www.amido.com/"
                    });
                    
                    c.CustomSchemaIds(type => type.FriendlyId(false));
                    c.DescribeAllEnumsAsStrings();
                    c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");

                    // Show only operations where route starts with
                    //c.DocumentFilter<BasePathFilter>("/v1");
                    // Sets the basePath property in the Swagger document generated
                    c.DocumentFilter<BasePathFilter>(pathBase);

                    //Set default tags, shows on top, non defined tags appears at bottom
                    c.DocumentFilter<SwaggerDocumentTagger>(new Tag[] {
                            new Tag { Name = "Menu" },
                            new Tag { Name = "Category" },
                            new Tag { Name = "Item" }
                        }, new string[] { });

                    // Include DataAnnotation attributes on Controller Action parameters as Swagger validation rules (e.g required, pattern, ..)
                    // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                    c.OperationFilter<GeneratePathParamsValidationFilter>();

                    //By Default, all endpoints are grouped by the controller name
                    //We want to Group by Api Group first, then by controller name if not provided
                    c.TagActionsBy((api) => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });

                    c.DocInclusionPredicate((docName, apiDesc) => { return true; });
                })
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info
                    {
                        Version = "all",
                        Title = "Menu API",
                        Description = "APIs used to interact and manage menus for a restaurant",
                        Contact = new Contact()
                        {
                            Name = "Amido",
                            Url = "https://github.com/amido/stacks-dotnet",
                            Email = "stacks@amido.com"
                        },
                        TermsOfService = "http://www.amido.com/"
                    });

                    c.CustomSchemaIds(type => type.FriendlyId(false));
                    c.DescribeAllEnumsAsStrings();
                    c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");

                    // Show only operations where route starts with
                    c.DocumentFilter<VersionPathFilter>("/v1");
                    // Sets the basePath property in the Swagger document generated
                    c.DocumentFilter<BasePathFilter>(pathBase);

                    // Include DataAnnotation attributes on Controller Action parameters as Swagger validation rules (e.g required, pattern, ..)
                    // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                    c.OperationFilter<GeneratePathParamsValidationFilter>();
                })
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v2", new Info
                    {
                        Version = "all",
                        Title = "Menu API",
                        Description = "APIs used to interact and manage menus for a restaurant",
                        Contact = new Contact()
                        {
                            Name = "Amido",
                            Url = "https://github.com/amido/stacks-dotnet",
                            Email = "stacks@amido.com"
                        },
                        TermsOfService = "http://www.amido.com/"
                    });

                    c.CustomSchemaIds(type => type.FriendlyId(false));
                    c.DescribeAllEnumsAsStrings();
                    c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");

                    // Show only operations where route starts with
                    c.DocumentFilter<VersionPathFilter>("/v2");
                    // Sets the basePath property in the Swagger document generated
                    c.DocumentFilter<BasePathFilter>(pathBase);

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
                .UsePathBase(pathBase)
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.DisplayOperationId();
                    
                    c.SwaggerEndpoint("all/swagger.json", "Menu (all)");
                    c.SwaggerEndpoint("v1/swagger.json", "Menu (version 1)");
                    c.SwaggerEndpoint("v2/swagger.json", "Menu (version 2)");
                });
        }
    }
}
