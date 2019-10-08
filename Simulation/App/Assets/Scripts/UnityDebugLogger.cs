using Common.HubClient.HubClient;
using UnityEngine;

public class UnityDebugLogger : IDebugLogger
{
    public void Log(object message)
    {
        Debug.Log(message);
    }

    public void LogError(object message)
    {
        Debug.LogError(message);
    }

    public void LogWarning(object message)
    {
        Debug.LogWarning(message);
    }
}
