using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.HubClient
{
    public interface IClient
    {
        HubConnection Connection { get; set; }

        string Address { get; set; }

        string Name { get; set; }

        Task Connect();

        Task Send<T>(string method, T message);

        Task SendAsync<T>(string method, T message);

        void WaitForConnection();
    }
}
