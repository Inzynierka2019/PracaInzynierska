namespace Broker
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Configuration class
    /// </summary>
    internal class Configuration
    {
        private readonly IConfiguration config;

        public Dictionary<string, string> Endpoints { get; set; }

        private string ConfigFile { get; set; }

        public Configuration(IConfiguration config)
        {
        }
    }
}
