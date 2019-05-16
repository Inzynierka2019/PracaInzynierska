namespace Broker.Tests
{
    using System;
    using System.Threading;
    using Broker.Communication;
    using Broker.Models;

    public class Program
    {
        static void Main(string[] args)
        {
            TestConnection();
            Console.ReadKey();
        }

        private static async void TestConnection()
        {
            using (var client = new SimClient())
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        var m = new Message { PosX = i, PosY = j };
                        await client.Send(m);
                        Thread.Sleep(100);
                    }
                }
            }
        }
    }
}
