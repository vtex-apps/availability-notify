using AvailabilityNotify.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vtex.Api.Context;

namespace Vtex
{
    public class StartupExtender
    {
        // This method is called inside Startup's constructor
        // You can use it to build a custom configuration
        public void ExtendConstructor(IConfiguration config, IWebHostEnvironment env)
        {

        }

        // This method is called inside Startup.ConfigureServices()
        // Note that you don't need to call AddControllers() here
        public void ExtendConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IVtexEnvironmentVariableProvider, VtexEnvironmentVariableProvider>();
            services.AddSingleton<IAvailabilityRepository, AvailabilityRepository>();
            services.AddSingleton<IVtexAPIService, VtexAPIService>();
            services.AddHttpContextAccessor();
            services.AddHttpClient();
        }

        // This method is called inside Startup.Configure() before calling app.UseRouting()
        public void ExtendConfigureBeforeRouting(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }

        // This method is called inside Startup.Configure() before calling app.UseEndpoint()
        public void ExtendConfigureBeforeEndpoint(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }

        // This method is called inside Startup.Configure() after calling app.UseEndpoint()
        public void ExtendConfigureAfterEndpoint(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
    }
}