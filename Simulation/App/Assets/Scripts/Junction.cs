using PathCreation;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class Junction : MonoBehaviour
{
    [HideInInspector]
    public ConcurrentBag<Road> entries = new ConcurrentBag<Road>();
    [HideInInspector]
    public List<Road> exits = new List<Road>();

    [HideInInspector]
    public List<Junction> consequent = new List<Junction>();
    
    private Task trafficLightsControllerThread;
    private CancellationTokenSource JunctionCancellationTokenSource = new CancellationTokenSource(); 

    public void Start()
    {
        trafficLightsControllerThread = Task.Factory.StartNew(() =>
        {
            CancellationToken cancellationToken = JunctionCancellationTokenSource.Token;
            try
            {
                var rand = new System.Random();
                do
                {
                    entries.ToList().ForEach(e =>
                    {
                        SimulationManager.ScheduleTaskOnMainThread(() => e.endNode.ChangeLightsToRed());
                    });

                    SimulationManager.ScheduleTaskOnMainThread(() =>
                    {
                        if (!entries.IsEmpty)
                            entries.ToArray().ElementAt(rand.Next(0, entries.Count)).endNode.ChangeLightsToGreen();
                    });

                    cancellationToken.WaitHandle.WaitOne(3500);
                    cancellationToken.ThrowIfCancellationRequested();
                } while (true);

            }catch(Exception e)
            {
                if (!(e is OperationCanceledException))
                    Debug.Log($"TrafficLightsControllerThread exception: {e.ToString()}");
            }
        }, TaskCreationOptions.LongRunning);
    }

    void Update()
    {
        if (!Application.isPlaying && transform.hasChanged)
        {
            SimulationManager.JunctionManager.RebuildRoads();
            transform.hasChanged = false;
        }
    }
    public void OnApplicationQuit()
    {
        JunctionCancellationTokenSource.Cancel();
        try
        {
            trafficLightsControllerThread.Wait();
        }catch(OperationCanceledException)
        {
            //Do nothing.It's an expected behaviour
        }
    }

    // Works only if Application.isPlaying
    void OnMouseDown()
    {
        Node sourceNode = exits[Random.Range(0, exits.Count)].startNode;
        SimulationManager.VehicleManager.Create(sourceNode, sourceNode.consequent.Keys.ToList()[Random.Range(0, sourceNode.consequent.Count)]);
    }

    public void ClearConnectionsAndPaths()
    {
        foreach (var r in entries.Concat(exits))
        {
            if (r != null)
                SimulationManager.RoadManager.Delete(r);
        }
        //clear entries
        var newEntries = new ConcurrentBag<Road>();
        Interlocked.Exchange<ConcurrentBag<Road>>(ref entries, newEntries);

        exits.Clear();
    }

    public void AddConsequent(Junction successor)
    {
        if (SimulationManager.RoadManager.Create(this, successor) != null)
            consequent.Add(successor);
    }
}
