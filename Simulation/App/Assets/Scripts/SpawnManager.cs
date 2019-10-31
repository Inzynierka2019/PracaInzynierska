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

    List<float>[] distributions;
    float[] weightSums;

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

    void Start()
    {
        StartCoroutine(Spawning());
    }

    Node ChooseNode(int spawnType)
    {
        float threshold = Random.Range(0, weightSums[spawnType]);
        return SimulationManager.RoadManager.roads.ElementAt(distributions[spawnType].FindIndex(0, p => p > threshold)).GetRandomNode();
    }

    IEnumerator Spawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(.2f);
            SimulationManager.VehicleManager.Create(ChooseNode(0), ChooseNode(1));
        }
    }
}
