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

public class Junction : MonoBehaviour
{
    public ConcurrentBag<Road> entries = new ConcurrentBag<Road>();
    public List<Road> exits = new List<Road>();

    public List<Junction> consequent = new List<Junction>();
    private Task trafficLightsControllerThread;
    private bool isApplicationClosing = false;

    public void ClearConnectionsAndPaths()
    {
        foreach (var r in exits)
        {
            DestroyImmediate(r.gameObject);
        }
        entries = new ConcurrentBag<Road>();
        exits.Clear();
    }

    public void OnApplicationQuit()
    {
        isApplicationClosing = true;
        trafficLightsControllerThread.Wait();
    }

    public void Start()
    {
        trafficLightsControllerThread = Task.Factory.StartNew(() =>
        {
            try
            {
                var rand = new System.Random();
                while(!isApplicationClosing)
                {
                    entries.ToList().ForEach(e =>
                    {
                        SimulationManager.ScheduleTaskOnMainThread(() => e.endNode.ChangeLightsToRed());
                    });

                    SimulationManager.ScheduleTaskOnMainThread(() =>
                    {
                        if(!entries.IsEmpty)
                            entries.ToArray().ElementAt(rand.Next(0, entries.Count)).endNode.ChangeLightsToGreen();
                    });
                    Thread.Sleep(3500);
                }

            }catch(Exception e)
            {
                Debug.Log($"TrafficLightsControllerThread exception: {e.ToString()}");
            }
        }, TaskCreationOptions.LongRunning);
    }

    public void OnMouseDown()
    {
        Debug.Log($"Creating vehicle on {name}");
        GameObject newVehicle = Instantiate(Resources.Load<GameObject>("VehiclePrefab"));
        Node sourceNode = exits[Random.Range(0, exits.Count)].startNode;

        newVehicle.GetComponent<Vehicle>().currentIntermidiateTarget = sourceNode.consequent.Keys.ToList()[Random.Range(0, sourceNode.consequent.Count)];
        newVehicle.transform.position = sourceNode.transform.position;
        sourceNode.GetComponent<Node>().vehicles.Add(newVehicle.GetComponent<Vehicle>());
    }

    private VertexPath CreateVertexPath(Vector3[] points)
    {
        BezierPath bezierPath = new BezierPath(points, isClosed: false, PathSpace.xy);
        return new VertexPath(bezierPath);
    }

    public void AddConsequent(Junction consequent)
    {
        var road = Instantiate(Resources.Load<GameObject>("RoadPrefab")).GetComponent<Road>();
        road.Create(this, consequent);
        SimulationManager.PlaceObjectInContainer(road.gameObject, SimulationManager.Containers.RoadsContainer);
    }
}
