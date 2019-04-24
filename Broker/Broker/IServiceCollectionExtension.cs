namespace Broker
{
    using Broker.Communication;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// IServiceCollection extension class
    /// </summary>
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection Services(this IServiceCollection services)
        {
            services.AddSignalR();

            return services;
        }
    }
}
