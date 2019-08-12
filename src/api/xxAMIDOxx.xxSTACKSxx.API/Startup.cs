using System;
using System.IO;
using Amido.Stacks.API.Swagger.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using xxAMIDOxx.xxSTACKSxx.API.Models;

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
        // Add dependent service required by the application
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services
                //.AddMvc()
                .AddMvcCore(
                    options =>
                    {
                        options.AllowValidatingTopLevelNodes = true;
                        options.InputFormatters.Clear();
                    }
                )
                .AddApiExplorer()
                .AddAuthorization()
                .AddDataAnnotations()
                .AddFormatterMappings()
                .AddJsonFormatters(options => options.ContractResolver = new CamelCasePropertyNamesContractResolver())
                .AddCors()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opts =>
                {
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opts.SerializerSettings.Converters.Add(new StringEnumConverter(typeof(CamelCaseNamingStrategy)));
                })
            ;

            //Access HttpContext in ASP.NET Core: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-2.2
            services.AddHttpContextAccessor();

            AddSwagger(services);
        }

        private void AddSwagger(IServiceCollection services)
        {
            services

                //Add swagger for all endpoints without any filter
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
                    c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{typeof(CreateOrUpdateMenu).Assembly.GetName().Name}.xml");

                    // Sets the basePath property in the Swagger document generated
                    c.DocumentFilter<BasePathFilter>(pathBase);

                    //Set default tags, shows on top, non defined tags appears at bottom
                    c.DocumentFilter<SwaggerDocumentTagger>(new Tag[] {
                            new Tag { Name = "Menu" },
                            new Tag { Name = "Category" },
                            new Tag { Name = "Item" }
                        }, new string[] { });

                    // Include DataAnnotation attributes on Controller Action parameters as Swagger validation rules (e.g required, pattern, ..)
                    c.OperationFilter<GeneratePathParamsValidationFilter>();

                    //By Default, all endpoints are grouped by the controller name
                    //We want to Group by Api Group first, then by controller name if not provided
                    c.TagActionsBy((api) => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });

                    c.DocInclusionPredicate((docName, apiDesc) => { return true; });
                })

                //Add swagger for v1 endpoints only
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info
                    {
                        Version = "v1",
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
                    c.OperationFilter<GeneratePathParamsValidationFilter>();
                })

                //Add swagger for v2 endpoints only
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v2", new Info
                    {
                        Version = "v2",
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
                    c.OperationFilter<GeneratePathParamsValidationFilter>();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Configure the pipeline with middlewares
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
