namespace Common.HubClient.Tests
{
    using System;
    using Common.HubClient.HubClient;

    public class ConsoleLogger : IDebugLogger
    {
        public void Log(object message)
        {
            Console.WriteLine(message);
        }

        public void LogError(object message)
        {
            Console.WriteLine(message);
        }

        public void LogWarning(object message)
        {
            Console.WriteLine(message);
        }
    }
}
