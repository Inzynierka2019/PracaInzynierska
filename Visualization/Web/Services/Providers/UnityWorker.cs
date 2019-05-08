using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Exceptions;
using Web.Services.Hubs;

namespace Web.Services
{
    public class UnityWorker : IUnityWorker
    {
        public UnityState State { get; set; }

        private static readonly object sync = new object();
        private readonly ISignalLogger<SimHub> Log;

        public UnityWorker(ISignalLogger<SimHub> log)
        {
            this.Log = log;
            Log.Info($"Unity worker started!");

            lock (sync)
            {
                this.State = UnityState.NOT_ESTABLISHED;
            }

            try
            {
                /// TODO:
                /// establish connection with Broker client.
            }
            catch
            {
                Log.Error("Exit!");
                throw new UnityConnectionException();
            }
            finally
            {
                this.LogState();
            }
        }

        public void LogState()
        {
            if(this.State.Equals(UnityState.ACTIVE))
            {
                Log.Info("Unity connection is active.");
            }
            else if(this.State.Equals(UnityState.NOT_ESTABLISHED))
            {
                Log.Info("Unity connection is not established.");
            }
        }
    }
}
