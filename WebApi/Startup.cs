using CleanArchitecture.Application.Core;
using CleanArchitecture.Application.Core.Services;
using CleanArchitecture.Application.Service;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi.Core.DependencyInjection;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureContainer(ServiceRegistry services)
        {
            services.AddControllers();
            // Also exposes Lamar specific registrations
            // and functionality
            services.Scan(s =>
            {
                s.LookForRegistries();
                s.TheCallingAssembly();
                s.AssembliesAndExecutablesFromApplicationBaseDirectory(a => a.FullName.Contains("CleanArchitecture"));
                s.AssemblyContainingType(typeof(Startup));
                s.WithDefaultConventions();
            });

            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            IoC.SetLocatorProvider(app.ApplicationServices);

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}