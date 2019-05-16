namespace Broker.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Broker.Communication;

    public class Program
    {
        static void Main(string[] args)
        {
            TestConnection();
        }

        private static async void TestConnection()
        {
            using (var client = new DebugClient())
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(200);
                    Console.Write($"{i}. \r");
                    await client.SendAsync($"[{i}] Hello there, General Kenobi.");
                }
            }
        }
    }
}
