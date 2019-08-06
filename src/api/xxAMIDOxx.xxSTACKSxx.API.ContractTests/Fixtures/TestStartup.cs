using Amido.Stacks.Application.CQRS.ApplicationEvents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using xxAMIDOxx.xxSTACKSxx.Application.Integration;

namespace xxAMIDOxx.xxSTACKSxx.API.ContractTests.Fixtures
{

    public class TestStartup : Startup
    {
        public TestStartup(IHostingEnvironment env, IConfiguration configuration) : base(env, configuration)
        {
        }


        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ProviderStateMiddleware>();

            AddMocks(services);

            base.ConfigureServices(services);
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ProviderStateMiddleware>();
            base.Configure(app, env);
        }

        public void AddMocks(IServiceCollection services)
        {
            services.AddSingleton<IMenuRepository>((svc) => Substitute.For<IMenuRepository>());
            services.AddSingleton<IApplicationEventPublisher>((svc) => Substitute.For<IApplicationEventPublisher>());
        }
    }
}
