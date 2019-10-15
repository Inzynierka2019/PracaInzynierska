namespace Web.Api
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Serilog;

    using Web.Logic.Hubs;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .WriteTo.Console()
            .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSignalR(routes =>
            {
                routes.MapHub<UIHub>("/UIHub");
            });

            app.UseWebSockets();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "WebUI";
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            Log.Information("Visualization App is starting...");
        }
    }
}
