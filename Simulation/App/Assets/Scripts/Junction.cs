using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class Junction : MonoBehaviour
{
    [HideInInspector]
    public List<Road> entries = new List<Road>();
    [HideInInspector]
    public List<Road> exits = new List<Road>();

    [HideInInspector]
    public List<Junction> consequent = new List<Junction>();

    void Update()
    {
        if (!Application.isPlaying && transform.hasChanged)
        {
            SimulationManager.JunctionManager.RebuildRoads();
            transform.hasChanged = false;
        }
    }

    // działa tylko w Application.isPlaying
    void OnMouseDown()
    {
        Node sourceNode = exits[Random.Range(0, exits.Count)].startNode;
        SimulationManager.VehicleManager.Create(sourceNode, sourceNode.consequent.Keys.ToList()[Random.Range(0, sourceNode.consequent.Count)]);
    }

    public void ClearConnectionsAndPaths()
    {
        foreach (var r in entries.Concat(exits))
        {
            if (r != null)
                SimulationManager.RoadManager.Delete(r);
        }
        entries.Clear();
        exits.Clear();
    }

    public void AddConsequent(Junction successor)
    {
        if (SimulationManager.RoadManager.Create(this, successor) != null)
            consequent.Add(successor);
    }
}
