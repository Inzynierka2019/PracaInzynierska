using Common.Communication;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    private readonly AppConnector appConnector = new AppConnector(new UnityDebugLogger(), "https://localhost:5001/UIHub");
    private static ConcurrentQueue<Action> MainThreadTaskQueue = new ConcurrentQueue<Action>();

    public SimulationManager()
    {
        // This line is necessary to actively ignore security concerns involving Mono certificate trust issues.
        ServicePointManager.ServerCertificateValidationCallback += (p1, p2, p3, p4) => true;
    }

    void OnApplicationQuit()
    {
        // disconnects from web server and informs that app has closed.
        this.appConnector.Dispose();
    }

    public static void ScheduleTaskOnMainThread(Action action)
    {
        MainThreadTaskQueue.Enqueue(action);
    }

    void Start()
    {
        RecreatePaths();
        this.appConnector.KeepAlive();
    }

    public void Update()
    {
        while (!MainThreadTaskQueue.IsEmpty)
        {
            if (MainThreadTaskQueue.TryDequeue(out Action action))
                action();
        }
    }

    // ten system totalnie absolutnie do zmiany
    public enum Containers
    {
        BigNodesContainer,
        SmallNodesContainer,
        RoadsContainer
    }

    public static GameObject GetObjectsContainer(Containers container)
    {
        var nodesContainer = GameObject.Find(container.ToString());
        if (nodesContainer == null)
        {
            nodesContainer = new GameObject();
            nodesContainer.name = container.ToString();
        }
        return nodesContainer;
    }

    public static void PlaceObjectInContainer(GameObject gameObject, Containers container)
    {
        gameObject.transform.parent = GetObjectsContainer(container).transform;
    }

    public static void RecreatePaths()
    {
        Debug.Log("Recreating paths. Please hold");
        var smallNodes = GetObjectsContainer(Containers.SmallNodesContainer);
        GameObject.DestroyImmediate(smallNodes);

        var bigNodes = GetObjectsContainer(Containers.BigNodesContainer);
        var cache = new Dictionary<Junction, List<Junction>>();
        foreach (Transform node in bigNodes.transform)
        {
            var nodeComponent = node.GetComponent<Junction>();
            nodeComponent.ClearConnectionsAndPaths();
            cache.Add(nodeComponent, nodeComponent.consequent);
            nodeComponent.consequent = new List<Junction>();
        }

        foreach (Transform node in bigNodes.transform)
        {
            var nodeComponent = node.GetComponent<Junction>();
            var nodeNeighbours = cache[nodeComponent];
            foreach (var neighbour in nodeNeighbours)
                nodeComponent.AddConsequent(neighbour);
        }
        Debug.Log("Recreating paths. Done.");
    }
}
