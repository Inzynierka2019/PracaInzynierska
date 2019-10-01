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
    public List<Road> entries = new List<Road>();
    public List<Road> exits = new List<Road>();

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
        Debug.Log($"Creating vehicle on {name}");
        GameObject newVehicle = Instantiate(Resources.Load<GameObject>("VehiclePrefab"));
        Node sourceNode = exits[Random.Range(0, exits.Count)].startNode;

        newVehicle.GetComponent<Vehicle>().currentIntermidiateTarget = sourceNode.consequent.Keys.ToList()[Random.Range(0, sourceNode.consequent.Count)];
        newVehicle.transform.position = sourceNode.transform.position;
        sourceNode.GetComponent<Node>().vehicles.Add(newVehicle.GetComponent<Vehicle>());
    }

    public void ClearConnectionsAndPaths()
    {
        foreach (var r in entries.Concat(exits))
        {
            if(r != null)
                DestroyImmediate(r.gameObject);
        }
        entries.Clear();
        exits.Clear();
    }

    private VertexPath CreateVertexPath(Vector3[] points)
    {
        BezierPath bezierPath = new BezierPath(points, isClosed: false, PathSpace.xy);
        return new VertexPath(bezierPath);
    }

    public void AddConsequent(Junction consequent)
    {
        if(consequent != null)
            Instantiate(Resources.Load<GameObject>("RoadPrefab")).GetComponent<Road>().Create(this, consequent);
    }
}
