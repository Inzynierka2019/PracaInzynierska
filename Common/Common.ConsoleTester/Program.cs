namespace Common.ConsoleTester
{
    using System;
    using Common.Communication;
    using Common.Utils;

    class Program
    {
        static void Main(string[] args)
        {
            TestConnection();
            Console.ReadKey();
        }

        static void TestConnection()
        {
            new AppConnector().KeepAlive();
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
