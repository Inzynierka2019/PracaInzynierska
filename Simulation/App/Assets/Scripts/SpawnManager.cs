using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static readonly string[] spawnTypes =
    {
        "osiedle",
        "praca",
        "sklepy",
        "atrakcje"
    };

    public static readonly int spawnTypeCount = spawnTypes.Length;
}
