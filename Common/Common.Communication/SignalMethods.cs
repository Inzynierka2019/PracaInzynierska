namespace Common.Communication
{
    using Common.Models;
    using System;

    /// <summary>
    /// The list of signal methods
    /// </summary>
    public class SignalMethods
    {
        public static readonly SignalMethod SignalForConsoleLogs
            = new SignalMethod(typeof(ConsoleLog), "SignalForConsoleLogs");

        public static readonly SignalMethod SignalForVehiclePopulation
            = new SignalMethod(typeof(VehiclePopulation), "SignalForVehiclePopulation");

        public static readonly SignalMethod SignalForUnityAppConnectionStatus
            = new SignalMethod(typeof(bool), "SignalForUnityAppConnectionStatus");
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
