﻿namespace Common.ConsoleTester
{
    using System;
    using System.Threading;
    using Common.Communication;
    using Common.Utils;

    class Program
    {
        static void Main(string[] args)
        {
            TestConnection();
        }

        static void TestConnection()
        {
            Console.WriteLine("Starting [AppConnector] service...");
            Console.WriteLine("AppConnector sends 'keep-alive' messages to server every 2s.");

            using (var appConnector = new AppConnector())
            {
                appConnector.KeepAlive();
                Thread.Sleep(15000);
            }
            Console.WriteLine("AppConnector exited.");
        }

        static void TestSignalMethod()
        {
            Console.WriteLine("Started test connection.");
            var client = new HubClient("TestClient");

            var timer = new ActionTimer(
                async () => await client.Send(
                    SignalMethods.SignalForVehiclePopulation.Method,
                    DummyDataManager.GetVehiclePopulation()
                    ), new TimeSpan(0,0,1));
        }
    }
}
