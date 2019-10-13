using Common.Communication;
using Common.Models;
using Libraries.Web;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

[ExecuteInEditMode]
public class SimulationManager : MonoBehaviour
{
    static SimulationManager instance = null;

    [SerializeField] JunctionManager junctionManager;
    [SerializeField] RoadManager roadManager;
    [SerializeField] VehicleManager vehicleManager;

    private readonly AppConnector appConnector = new AppConnector(new UnityDebugLogger(), "https://localhost:5001/UIHub");
    private static ConcurrentQueue<Action> MainThreadTaskQueue = new ConcurrentQueue<Action>();

    private static DataAggregationModule dataAggregationModule;

    public static JunctionManager JunctionManager
    {
        get => instance?.junctionManager;
        set { if (instance != null) instance.junctionManager = value; }
    }

    public static RoadManager RoadManager
    {
        get => instance?.roadManager;
        set { if (instance != null) instance.roadManager = value; }
    }

    public static VehicleManager VehicleManager
    {
        get => instance?.vehicleManager;
        set { if (instance != null) instance.vehicleManager = value; }
    }

    public static void ScheduleTaskOnMainThread(Action action)
    {
        MainThreadTaskQueue.Enqueue(action);
    }

    public static void Rebuild()
    {
        Debug.Log("Rebuilding. Please hold");
        JunctionManager.RebuildRoads();
        Debug.Log("Rebuilding. Done.");
    }

    void Start()
    {
        if (instance != null)
            Debug.LogError("Too many simulation managers! Leave just one in hierarchy.");
        instance = this;

        if (junctionManager == null)
        {
            GameObject go = new GameObject("JunctionManager");
            go.transform.SetParent(transform);
            junctionManager = go.AddComponent<JunctionManager>();
        }
        if (roadManager == null)
        {
            GameObject go = new GameObject("RoadManager");
            go.transform.SetParent(transform);
            roadManager = go.AddComponent<RoadManager>();
        }
        if (vehicleManager == null)
        {
            GameObject go = new GameObject("VehicleManager");
            go.transform.SetParent(transform);
            vehicleManager = go.AddComponent<VehicleManager>();
        }

        Rebuild();
        this.appConnector.KeepAlive();
        dataAggregationModule =
            gameObject.AddComponent<DataAggregationModule>();
        dataAggregationModule.Init(vehicleManager);
    }

    void Update()
    {
        while (Application.isPlaying && !MainThreadTaskQueue.IsEmpty)
        {
            if (MainThreadTaskQueue.TryDequeue(out Action action))
            {
                action();
            }
        }
    }

    public SimulationManager()
    {
        // This line is necessary to actively ignore security concerns involving Mono certificate trust issues.
        ServicePointManager.ServerCertificateValidationCallback += (p1, p2, p3, p4) => true;
    }

    void OnApplicationQuit()
    {
        // disconnects from web server and informs that app has closed.
        this.appConnector.Dispose();
        dataAggregationModule.StopAndWaitForShutdown();
    }
}
