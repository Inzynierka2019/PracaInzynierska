using Common.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public static readonly string[] spawnTypes =
    {
        "Osiedle",
        "Praca",
        "Sklepy",
        "Szkoły",
        "Atrakcje",
        "Wyjazd"
    };

    public static readonly int spawnTypeCount = spawnTypes.Length;

    public class RouteType
    {
        public string name;
        public int spawnType;
        public int targetType;
        public float routeTypeWeight;

        public RouteType(string n, string spawnName, string targetName)
        {
            name = n;
            spawnType = Array.IndexOf(spawnTypes, spawnName);
            targetType = Array.IndexOf(spawnTypes, targetName);
            routeTypeWeight = 0f;
        }
    }

    public static readonly RouteType[] RouteTypes =
    {
        new RouteType("Do pracy", "Osiedle", "Praca"),
        new RouteType("Z pracy", "Praca", "Osiedle"),
        new RouteType("Do pracy przez szkołę", "Osiedle", "Szkoły"),
        new RouteType("Do pracy przez szkołę", "Szkoły", "Praca"),
        new RouteType("Z pracy przez szkołę", "Praca", "Szkoły"),
        new RouteType("Z pracy przez szkołę", "Szkoły", "Osiedle"),
        new RouteType("Zakupy", "Osiedle", "Sklepy"),
        new RouteType("Zakupy", "Sklepy", "Osiedle"),
        new RouteType("Turystycznie", "Wyjazd", "Atrakcje"),
        new RouteType("Turystycznie", "Atrakcje", "Wyjazd"),
        new RouteType("Przejazdem", "Wyjazd", "Wyjazd")
    };

    public static readonly int routeTypeCount = RouteTypes.Length;


    List<float>[] distributions;
    float[] weightSums;

    float[] routeTypeWeights = Enumerable.Repeat(0f, routeTypeCount).ToArray();
    float routeTypeWeightsSum = routeTypeCount;
    float spawnPeriod = 1f;
    int vehicleCountMaximum;
    int vehicleCount;

    public void CalculateDistribution()
    {
        distributions = Enumerable.Range(0, spawnTypeCount).Select(i => new List<float>()).ToArray();
        weightSums = Enumerable.Repeat(0f, spawnTypeCount).ToArray();
        foreach (Road r in SimulationManager.RoadManager.roads)
        {
            int length = r.nodes.Count;
            for (int i = 0; i < spawnTypeCount; i++)
            {
                weightSums[i] += r.spawnWeights[i] * length;
                distributions[i].Add(weightSums[i]);
            }
        }
    }

    public void SetParameters(ScenePreference scenePreference)
    {
        foreach (VehicleSpawnChance vsc in scenePreference.vehicleSpawnChances)
        {
            RouteType[] selectedRoutes = RouteTypes.Where(rt => rt.name == vsc.routeType).ToArray();
            int selectedCount = selectedRoutes.Length;
            if (selectedCount > 0)
            {
                foreach(var rt in selectedRoutes)
                {
                    rt.routeTypeWeight = (float)vsc.spawnChance / selectedCount;
                }
            }
            else
            {
                Debug.LogError("Could not find route type specified in configuration.");
                Debug.LogError($"Route type name is '{vsc.routeType}'.");
            }
        }

        routeTypeWeightsSum = 0f;
        for(int i = 0; i < routeTypeCount; i++)
        {
            routeTypeWeights[i] = RouteTypes[i].routeTypeWeight;
            routeTypeWeightsSum += routeTypeWeights[i];
        }

        this.spawnPeriod = 1f / scenePreference.vehicleSpawnFrequency;
        this.vehicleCountMaximum = scenePreference.vehicleCountMaximum;

        DriverFactory.driverSpawnChances = scenePreference.driverSpawnChances;
    }

    void Start()
    {
        StartCoroutine(Spawning());
    }

    Road ChooseRoad(int spawnType, Road disqualifiedRoad = null)
    {
        Road result = null;
        for(int tries = 0; tries < 10; tries++)
        {
            float threshold = Random.Range(0, weightSums[spawnType]);
            int index = distributions[spawnType].FindIndex(p => p > threshold);
            if (index < 0)
                return null;
            result = SimulationManager.RoadManager.roads.ElementAt(index);
            if (result != disqualifiedRoad)
                break;
        }
        return result;
    }

    RouteType ChooseRouteType()
    {
        float threshold = Random.Range(0, routeTypeWeightsSum);
        for(int i = 0; i < routeTypeCount; i++)
        {
            threshold -= routeTypeWeights[i];
            if (threshold < 0)
                return RouteTypes[i];
        }
        return RouteTypes.Last();
    }

    IEnumerator Spawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnPeriod);
            if(vehicleCount < vehicleCountMaximum)
            {
                RouteType rt = ChooseRouteType();
                Road spawnRoad = ChooseRoad(rt.spawnType);
                Road targetRoad = ChooseRoad(rt.targetType, spawnRoad);
                if(SimulationManager.VehicleManager.Create(spawnRoad?.GetRandomNode(), targetRoad?.GetRandomNode(), rt.name))
                    vehicleCount++;
            }
        }
    }

    public void NotifyVehicleRouteFinished(Vehicle vehicle)
    {
        vehicleCount--;
        if (vehicle.IsSelected)
            vehicle.OnMouseUp();
        SimulationManager.dataAggregationModule.CreateDriverReport(vehicle);
    }
}
