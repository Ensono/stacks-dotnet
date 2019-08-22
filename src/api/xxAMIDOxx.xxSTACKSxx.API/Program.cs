using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;
using xxAMIDOxx.xxSTACKSxx.Infrastructure;

namespace xxAMIDOxx.xxSTACKSxx.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateLogger();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // this config file will be injected at runtime in the /app/config folder of the container. 
                    // Do not put any file in there
                    // the appsettings.json is not added to the image to avoid configuration conflicts 
                    // In the future we should set optional: false to force the user provide a config file
                    config.AddJsonFile($"config/appsettings.json", optional: true, reloadOnChange: false);
                })
                .UseStartup<Startup>()
                .UseSerilog()
                .ConfigureServices(DependencyRegistration.ConfigureStaticDependencies)
                .ConfigureServices(DependencyRegistration.ConfigureProductionDependencies)
            ;

        private static void CreateLogger()
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .Enrich.FromLogContext()
               //TODO: Configure from file
               // https://github.com/serilog/serilog-settings-configuration
               .WriteTo.ApplicationInsights(TelemetryConfiguration.Active, TelemetryConverter.Traces)
               //.WriteTo.ApplicationInsights(TelemetryConfiguration.Active, new CustomConverter())
               .WriteTo.Console()
               .CreateLogger();
        }
    }


    //TODO: We might need a custom Concerter when handling message queues
    public class CustomConverter : TraceTelemetryConverter
    {
        public override IEnumerable<ITelemetry> Convert(LogEvent logEvent, IFormatProvider formatProvider)
        {
            // first create a default TraceTelemetry using the sink's default logic
            // .. but without the log level, and (rendered) message (template) included in the Properties
            foreach (ITelemetry telemetry in base.Convert(logEvent, formatProvider))
            {
                // then go ahead and post-process the telemetry's context to contain the user id as desired
                if (logEvent.Properties.ContainsKey("UserId"))
                {
                    telemetry.Context.User.Id = logEvent.Properties["UserId"].ToString();
                }
                // post-process the telemetry's context to contain the operation id
                if (logEvent.Properties.ContainsKey("operation_Id"))
                {
                    telemetry.Context.Operation.Id = logEvent.Properties["operation_Id"].ToString();
                }
                // post-process the telemetry's context to contain the operation parent id
                if (logEvent.Properties.ContainsKey("operation_parentId"))
                {
                    telemetry.Context.Operation.ParentId = logEvent.Properties["operation_parentId"].ToString();
                }
                // typecast to ISupportProperties so you can manipulate the properties as desired
                ISupportProperties propTelematry = (ISupportProperties)telemetry;

                // find redundent properties
                var removeProps = new[] { "UserId", "operation_parentId", "operation_Id" };
                removeProps = removeProps.Where(prop => propTelematry.Properties.ContainsKey(prop)).ToArray();

                foreach (var prop in removeProps)
                {
                    // remove redundent properties
                    propTelematry.Properties.Remove(prop);
                }

                yield return telemetry;
            }
        }
    }

}
