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

    public static class Registry
    {
        public static void Configure(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins(
                        "http://localhost:4200",
                        "https://localhost:4200",
                        "http://+:5000",
                        "https://+:5001")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin() //without this doesnt work... fix
                    );
            });

            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddFilter("Microsoft", LogLevel.Warning);
                builder.AddFilter("System", LogLevel.Error);
                builder.AddFilter("Engine", LogLevel.Error);
                builder.AddLog4Net();
            });

            services.AddSignalR(hubOptions =>
            {
                hubOptions.HandshakeTimeout = TimeSpan.FromMinutes(10.0);
                hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(10.0);
                hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(10.0);
                hubOptions.EnableDetailedErrors = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
            services.AddSingleton<IMapper>(new Mapper(config));
            services.AddTransient<IProcessService, ProcessService>();
            services.AddTransient<ILog, Log>();
            services.AddTransient<IConsoleLogUpdater, ConsoleLogUpdater>();
        }
    }
}
