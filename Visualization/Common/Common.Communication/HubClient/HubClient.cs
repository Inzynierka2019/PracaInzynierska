namespace Common.Communication
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.SignalR.Client;
    using System.Reflection;
    using Common.HubClient.HubClient;

    public class HubClient : BaseClient
    {
        public List<SignalMethod> RegisteredMethods;

        /// <summary>
        /// Initializes a new instance of the HubClient class with default methods.
        /// </summary>
        /// <param name="name">The name of HubClient instance.</param>
        public HubClient(IDebugLogger logger, string address, string name = "Client") : base(name, address, logger)
        {
            base.Logger.Log($"{name} hub was created.");
            GetDefaultMethods();
            InitClient(RegisteredMethods);
        }

        /// <summary>
        /// Initializes a new instance of the HubClient class with one specified method.
        /// </summary>
        /// <param name="method">The method to be registered.</param>
        /// <param name="name">The name of HubClient instance.</param>
        public HubClient(SignalMethod method, IDebugLogger logger, string address, string name = "Client") : base(name, address, logger)
        {
            base.Logger.Log($"{name} hub was created.");
            InitClient(new List<SignalMethod>() { method });
        }

        /// <summary>
        /// Initializes a new instance of the HubClient class with the list of methods.
        /// </summary>
        /// <param name="method">The method to be registered.</param>
        /// <param name="name">The name of HubClient instance.</param>
        public HubClient(List<SignalMethod> methods, IDebugLogger logger, string address, string name = "Client") : base(name, address, logger)
        {
            base.Logger.Log($"{name} hub was created.");
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
            /* uncomment for debugging purposes to log all sent messages. */
            //base.Connection.On<T>(method, new Action<T>(LogMessage));
            /*                                                            */
            base.Connection.On<T>(method, null);
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
            base.Logger.Log(message);
        }
    }
}
