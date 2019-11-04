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


    public struct RouteType
    {
        public string name;
        public int spawnType;
        public int targetType;

        public RouteType(string n, string spawnName, string targetName)
        {
            name = n;
            spawnType = Array.IndexOf(spawnTypes, spawnName);
            targetType = Array.IndexOf(spawnTypes, targetName);
        }
    }

    public static readonly RouteType[] routeTypes =
    {
        new RouteType("Do pracy", "Osiedle", "Praca"),
        new RouteType("Na zakupy", "Osiedle", "Sklepy"),
        new RouteType("Turystycznie", "Osiedle", "Atrakcje")
    };

    public static readonly int routeTypeCount = routeTypes.Length;


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

    public void SetParameters(float[] routeTypeWeights, float vehicleFrequency)
    {
        if (routeTypeWeights.Length != routeTypeCount)
            Debug.LogError("There is different number of declared route types and incoming parameters.");

        routeTypeWeightsSum = 0f;
        for(int i = 0; i < routeTypeCount; i++)
        {
            this.routeTypeWeights[i] = routeTypeWeights[i];
            routeTypeWeightsSum += routeTypeWeights[i];
        }

        spawnPeriod = 1f / vehicleFrequency;
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
                return routeTypes[i];
        }
        return routeTypes.Last();
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
