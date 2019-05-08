using Broker.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Services.Hubs
{
    public class SimHub : Hub
    {
        private readonly ISignalLogger<SimHub> log;

        public SimHub(ISignalLogger<SimHub> log) : base()
        {
            this.log = log;
        }

        public Task SimulationMessage(Message message)
        {
            log.Info(message.ToString());
            return Clients.All.SendAsync("SimulationMessage", message);
        }
    }
}
