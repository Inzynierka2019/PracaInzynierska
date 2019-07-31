using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.HubClient.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            TestConnection();
            Console.ReadKey();
        }

        private static async void TestConnection()
        {
            using (var client = new HubClient("HubClient"))
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
