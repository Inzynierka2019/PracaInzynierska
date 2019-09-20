namespace Common.Communication
{
    using log4net;
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.SignalR.Client;
    using System.Reflection;

    public class HubClient : BaseClient
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HubClient));

        public List<SignalMethod> RegisteredMethods;

        /// <summary>
        /// Initializes a new instance of the HubClient class with default methods.
        /// </summary>
        /// <param name="name">The name of HubClient instance.</param>
        public HubClient(string name = "Client") : base(name)
        {
            Log.Info($"{name} hub was created.");
            GetDefaultMethods();
            InitClient(RegisteredMethods);
        }

        /// <summary>
        /// Initializes a new instance of the HubClient class with one specified method.
        /// </summary>
        /// <param name="method">The method to be registered.</param>
        /// <param name="name">The name of HubClient instance.</param>
        public HubClient(SignalMethod method, string name = "Client") : base(name)
        {
            Log.Info($"{name} hub was created.");
            InitClient(new List<SignalMethod>() { method });
        }

        /// <summary>
        /// Initializes a new instance of the HubClient class with the list of methods.
        /// </summary>
        /// <param name="method">The method to be registered.</param>
        /// <param name="name">The name of HubClient instance.</param>
        public HubClient(List<SignalMethod> methods, string name = "Client") : base(name)
        {
            Log.Info($"{name} hub was created.");
            InitClient(methods);
        }

        private void InitClient(List<SignalMethod> signalMethods)
        {
            MethodInfo method = this.GetType()
                .GetMethod(
                    "RegisterConnection",
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    CallingConventions.Any,
                    new Type[] { typeof(string) },
                    null);

            foreach (var m in signalMethods)
            {
                object[] param = new object[1] { m.Method };
                MethodInfo generic = method.MakeGenericMethod(m.Model);
                generic.Invoke(this, param);
            }
        }

        private void RegisterConnection<T>(string method)
        {
            base.Connection.On<T>(method, LogMessage);
        }

        private void GetDefaultMethods()
        {
            this.RegisteredMethods = new List<SignalMethod>()
            {
                SignalMethods.SignalForVehiclePopulation
            };
        }

        private void LogMessage<T>(T message)
        {
            Log.Debug(message);
        }
    }
}
