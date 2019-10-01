using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SimulationManager : MonoBehaviour
{
    static SimulationManager instance = null;

    [SerializeField] JunctionManager junctionManager;
    [SerializeField] RoadManager roadManager;
    [SerializeField] VehicleManager vehicleManager;

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

        if(junctionManager == null)
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
    }
}
