using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.HubClient
{
    public class SignalMethods
    {
        public static readonly SignalMethod SignalForConsoleLogs
            = new SignalMethod(typeof(ConsoleLog), "SignalForConsoleLogs");

        public static readonly SignalMethod SignalForVehiclePopulation
            = new SignalMethod(typeof(VehiclePopulation), "SignalForVehiclePopulation");

        public static readonly SignalMethod SignalForUnityAppConnectionStatus
            = new SignalMethod(typeof(bool), "UnityAppConnectionStatus");
    }

    public class SignalMethod
    {
        public Type Model { get; set; }
        
        public string Method { get; set; }

        public SignalMethod(Type model, string method)
        {
            this.Model = model;
            this.Method = method;
        }

        public override string ToString()
        {
            return this.Method;
        }
    }
}
