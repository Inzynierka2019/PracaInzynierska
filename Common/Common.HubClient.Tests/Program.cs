using System;
using System.Threading.Tasks;

namespace Common.HubClient.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => TestConnection());
            Console.ReadKey();
        }

        private static void TestConnection()
        {
            using (var client = new HubClient("TestClient"))
            {
                var timer = new DummyTimer(
                    async () => await client.Send(
                        SignalMethods.SignalForVehiclePopulation.Method, 
                        DummyDataManager.GetVehiclePopulation()
                        ));
            }
        }
    }
}
