
using Common.Models;
using Libraries.Web;
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
    private readonly AppUpdater appUpdater =
        new AppUpdater(new UnityDebugLogger(), "https://localhost:5001/UIHub");

    private Task communicationThreadHandle;
    private BlockingCollection<IMessage> messageQueue = new BlockingCollection<IMessage>();
    private CancellationToken cancellationToken;

    private void CommunicationThreadImpl()
    {
        try
        {
            foreach (var message in messageQueue.GetConsumingEnumerable(cancellationToken))
            {
                if (message is VehiclePopulation population)
                {
                    Debug.Log($"Sending: {population.ToString()}");
                    appUpdater.UpdateVehiclePopulationPositions(population);
                }
            }
        }
        catch(OperationCanceledException)
        {
            //Do nothing. It's an expected behaviour
        }
    }

    public void AddMessageToQueue(IMessage message)
    {
        messageQueue.Add(message);
    }

    public void WaitForShutdown()
    {
        communicationThreadHandle.Wait();
    }

}