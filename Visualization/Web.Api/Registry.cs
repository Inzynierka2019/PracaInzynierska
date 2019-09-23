namespace Web.Api
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using AutoMapper;

    using Web.Logic.Services;
    using Web.Utils.Mapping;
    using Web.Logic.WebUI;
    using Microsoft.Extensions.Configuration;
    using Web.Logic.Configuration;

    public static class Registry
    {
        public static void Configure(this IServiceCollection services, IConfiguration Configuration)
        {
            var unityConfig = new UnityConfiguration();
            Configuration.Bind("UnityApp", unityConfig);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:4200")
                    .Build()
                    );
            });

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddFilter("Microsoft", LogLevel.Warning);
                builder.AddFilter("System", LogLevel.Trace);
                builder.AddFilter("Engine", LogLevel.Error);
                builder.AddLog4Net();
            });

            services.AddSignalR(hubOptions =>
            {
                hubOptions.HandshakeTimeout = TimeSpan.FromMinutes(10.0);
                hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(10.0);
                hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(10.0);
                hubOptions.EnableDetailedErrors = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
            services.AddSingleton<IMapper>(new Mapper(config));
            services.AddSingleton<IUnityConfiguration>(unityConfig);
            services.AddTransient<IProcessService, ProcessService>();
            services.AddTransient<ILog, Log>();
            services.AddTransient<IConsoleLogUpdater, ConsoleLogUpdater>();
        }
    }
}
