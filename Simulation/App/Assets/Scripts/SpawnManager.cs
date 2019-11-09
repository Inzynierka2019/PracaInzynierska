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
        "Atrakcje"
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
            routeTypeWeight = 1f;
        }
    }

    public static readonly RouteType[] RouteTypes =
    {
        new RouteType("Do pracy", "Osiedle", "Praca"),
        new RouteType("Na zakupy", "Osiedle", "Sklepy"),
        new RouteType("Turystycznie", "Osiedle", "Atrakcje")
    };

    public static readonly int routeTypeCount = RouteTypes.Length;


    List<float>[] distributions;
    float[] weightSums;

    float[] routeTypeWeights = Enumerable.Repeat(1f, routeTypeCount).ToArray();
    float routeTypeWeightsSum = routeTypeCount;
    float spawnPeriod = 1f;

    public void CalculateDistribution()
    {
        distributions = Enumerable.Range(0, spawnTypeCount).Select(i => new List<float>()).ToArray();
        weightSums = Enumerable.Repeat(0f, spawnTypeCount).ToArray();
        foreach (Road r in SimulationManager.RoadManager.roads)
        {
            for (int i = 0; i < spawnTypeCount; i++)
            {
                weightSums[i] += r.spawnWeights[i];
                distributions[i].Add(weightSums[i]);
            }
        }
    }

    public void SetParameters(ScenePreference scenePreference)
    {
        if (scenePreference.vehicleSpawnChances.Count != routeTypeCount)
            Debug.LogError("There is different number of declared route types and incoming parameters.");

        routeTypeWeightsSum = 0f;
        int index = 0;

        foreach (var routeType in SpawnManager.RouteTypes)
        {
            var spawnOption = scenePreference.vehicleSpawnChances.Where(x => x.routeType == routeType.name).FirstOrDefault();
            if(spawnOption != null)
            {
                routeType.routeTypeWeight = spawnOption.spawnChance / 100.0f;
                this.routeTypeWeights[index] = routeType.routeTypeWeight;
                routeTypeWeightsSum += routeTypeWeights[index];
                index++;
            }
            else
            {
                Debug.LogError("Could not find route type specified in configuration.");
                Debug.LogError($"Route type name is '{routeType.name}'.");
            }
        }

        this.spawnPeriod = 1f / scenePreference.vehicleSpawnFrequency;
    }

    void Start()
    {
        StartCoroutine(Spawning());
    }

    Node ChooseNode(int spawnType)
    {
        float threshold = Random.Range(0, weightSums[spawnType]);
        return SimulationManager.RoadManager.roads.ElementAt(distributions[spawnType].FindIndex(0, p => p > threshold)).GetRandomNode();
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
            RouteType rt = ChooseRouteType();
            SimulationManager.VehicleManager.Create(ChooseNode(rt.spawnType), ChooseNode(rt.targetType));
        }
    }
}
