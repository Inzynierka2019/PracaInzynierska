using Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using UnityEngine;

[ExecuteInEditMode]
public class SimulationManager : MonoBehaviour
{
    static SimulationManager instance = null;

    [SerializeField] JunctionManager junctionManager;
    [SerializeField] RoadManager roadManager;
    [SerializeField] VehicleManager vehicleManager;
    [SerializeField] SpawnManager spawnManager;

    [HideInInspector]
    public ScenePreference ScenePreference { get; set; }

    private static ConcurrentQueue<Action> MainThreadTaskQueue = new ConcurrentQueue<Action>();

    public static DataAggregationModule dataAggregationModule;

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

    public static SpawnManager SpawnManager
    {
        get => instance?.spawnManager;
        set { if (instance != null) instance.spawnManager = value; }
    }

    public static void ScheduleTaskOnMainThread(Action action)
    {
        MainThreadTaskQueue.Enqueue(action);
    }

    public static void Rebuild()
    {
        Debug.Log("Rebuilding. Please hold");
        JunctionManager.RebuildRoads();
        SpawnManager.CalculateDistribution();
        Debug.Log("Rebuilding. Done.");
    }

    void Start()
    {
        if (instance != null)
            Debug.LogError("Too many simulation managers! Leave just one in hierarchy.");
        instance = this;

        this.LoadSimulationPreferences();

        Application.runInBackground = true;

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
        if (spawnManager == null)
        {
            GameObject go = new GameObject("SpawnManager");
            go.transform.SetParent(transform);
            spawnManager = go.AddComponent<SpawnManager>();
        }
        if(!(dataAggregationModule = gameObject.GetComponent<DataAggregationModule>()))
        {
            dataAggregationModule =
                gameObject.AddComponent<DataAggregationModule>();

        }

        Rebuild();
        dataAggregationModule.Init(vehicleManager);

        spawnManager.SetParameters(this.ScenePreference);
        Junction.trafficLightsPeriod = this.ScenePreference.trafficLightsPeriod;
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
        dataAggregationModule.StopAndWaitForShutdown();
    }

    void LoadSimulationPreferences()
    {
        var configName = "simulation-preferences.json";
        var configPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, configName);

        Debug.Log($"Config path: {configPath}");

        using (var reader = new StreamReader(configPath))
        {
            try
            {
                var json = reader.ReadToEnd();
                var simulationPreferences = JsonConvert.DeserializeObject<SimulationPreferences>(json);
                this.ScenePreference = simulationPreferences.scenePreferences;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
}
