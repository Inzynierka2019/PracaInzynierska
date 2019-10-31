using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    List<float[]> distribution;
    float[] weightSums;

    public void CalculateDistribution()
    {
        distribution = new List<float[]>();
        weightSums = Enumerable.Repeat(0f, spawnTypeCount).ToArray();
        foreach (Road r in SimulationManager.RoadManager.roads)
        {
            float[] step = new float[spawnTypeCount];

            for (int i = 0; i < spawnTypeCount; i++)
            {
                weightSums[i] += r.spawnWeights[i];
                step[i] = weightSums[i];
            }

            distribution.Add(step);
        }
    }
}
