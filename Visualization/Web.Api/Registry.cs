﻿namespace Web.Api
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;

    using AutoMapper;

    using Web.Logic.Services;
    using Web.Utils.Mapping;
    using Web.Api.WebUI;

    public static class Registry
    {
        public static void Configure(this IServiceCollection services)
        {
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
            services.AddTransient<IConsoleLogUpdater, ConsoleLogUpdater>();
        }
    }
}
