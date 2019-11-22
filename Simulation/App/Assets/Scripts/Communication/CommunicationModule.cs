using Common.Models;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

class CommunicationModule
{
    public CommunicationModule(CancellationToken cancellationToken)
    {
        communicationThreadHandle =
            Task.Factory.StartNew(CommunicationThreadImpl, TaskCreationOptions.LongRunning);

        this.cancellationToken = cancellationToken;
    }

    private readonly AppUpdater appUpdater = new AppUpdater();

    private Task communicationThreadHandle;
    private BlockingCollection<IMessage> messageQueue = new BlockingCollection<IMessage>();
    private CancellationToken cancellationToken;

    private async Task CommunicationThreadImpl()
    {
        await appUpdater.Start();

        try
        {
            foreach (var message in messageQueue.GetConsumingEnumerable(cancellationToken))
            {
                this.appUpdater.Update((dynamic)message);
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Communication thread was cancelled.");
        }
        catch (ThreadAbortException ex)
        {
            Debug.Log($"The thread has been aborted: {ex}");
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    public void AddMessageToQueue(IMessage message)
    {
        messageQueue.Add(message);
    }

    public async void WaitForShutdown()
    {
        communicationThreadHandle.Wait();
        await this.appUpdater.Stop();
    }
}