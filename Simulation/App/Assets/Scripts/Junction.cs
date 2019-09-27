using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Junction : MonoBehaviour
{
    public List<Road> entries = new List<Road>();
    public List<Road> exits = new List<Road>();

    public List<Junction> consequent = new List<Junction>();

    public void ClearConnectionsAndPaths()
    {
        //paths.Clear();
        foreach (var r in exits)
        {
            DestroyImmediate(r.gameObject);
        }
        entries.Clear();
        exits.Clear();
    }

    public void OnMouseDown()
    {
        Debug.Log($"Creating vehicle on {name}");
        GameObject newVehicle = Instantiate(Resources.Load<GameObject>("VehiclePrefab"));
        Node sourceNode = exits[Random.Range(0, exits.Count)].startNode;

        newVehicle.GetComponent<Vehicle>().currentIntermidiateTarget = sourceNode.consequent.Keys.ToList()[Random.Range(0, sourceNode.consequent.Count)];
        newVehicle.transform.position = sourceNode.transform.position;
        sourceNode.GetComponent<Node>().vehicles.Add(newVehicle.GetComponent<Vehicle>());
    }

    private VertexPath CreateVertexPath(Vector3[] points)
    {
        BezierPath bezierPath = new BezierPath(points, isClosed: false, PathSpace.xy);
        return new VertexPath(bezierPath);
    }

    public void AddConsequent(Junction consequent)
    {
        Instantiate(Resources.Load<GameObject>("RoadPrefab")).GetComponent<Road>().Create(this, consequent);
    }
}
