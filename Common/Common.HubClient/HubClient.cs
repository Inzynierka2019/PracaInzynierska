using Common.Models;
using log4net;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;
using System.Reflection;

namespace Common.HubClient
{
    public class HubClient : BaseClient
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HubClient));

        public HubClient(string name) : base(name)
        {
            MethodInfo method = typeof(HubClient).GetMethod("RegisterConnection");

            foreach (var m in GetMethods())
            {
                object[] param = new object[1] { m.Method };
                MethodInfo generic = method.MakeGenericMethod(m.Model);
                generic.Invoke(this, param);
            }
        }

        private void RegisterConnection<T>(string method)
        {
            //foreach (var m in GetMethods())
            //{
             // Type type = m.Model;
             //   MethodInfo generic = method.MakeGenericMethod(type);
             //   generic.Invoke(this, m.Method);
                base.Connection.On<dynamic>(method, LogMessage);
                //???
            //}
        }

        private List<SignalMethod> GetMethods()
        {
            return new List<SignalMethod>()
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
