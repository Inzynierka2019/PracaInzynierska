﻿namespace Web.Api
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;

    using Web.Logic.Services;
    using Web.Logic.WebUI;

    public static class Registry
    {
        public static void Configure(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "WebUI/dist/WebUI";
            });

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

            services.AddSignalR(hubOptions =>
            {
                hubOptions.HandshakeTimeout = TimeSpan.FromMinutes(10.0);
                hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(10.0);
                hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(10.0);
                hubOptions.EnableDetailedErrors = true;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IConsoleLogUpdater, ConsoleLogUpdater>();
            services.AddTransient<IProcessService, ProcessService>();
            services.AddTransient<ILog, Log>();
            services.AddSingleton<IUnityAppManager, UnityAppManager>();
            services.AddSingleton<IStatisticsService, StatisticsService>();
        }
    }
}
