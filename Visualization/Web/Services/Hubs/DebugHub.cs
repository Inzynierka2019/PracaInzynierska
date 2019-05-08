using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Services.Hubs
{
    public class DebugHub : Hub
    {
        private readonly ISignalLogger<DebugHub> log;

        public DebugHub(ISignalLogger<DebugHub> log) : base()
        {
            this.log = log;
        }

        public Task DebugMessage(string message)
        {
            log.Debug(message);
            return Clients.All.SendAsync("DebugMessage", message);
        }
    }
}
