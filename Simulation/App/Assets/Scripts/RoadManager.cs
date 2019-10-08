using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using PathCreation;

[ExecuteInEditMode]
public class RoadManager : MonoBehaviour
{
    public List<Road> roads;

    GameObject prefab, nodePrefab;

    void Awake()
    {
        roads = new List<Road>(GetComponentsInChildren<Road>());

        prefab = Resources.Load<GameObject>("RoadPrefab");
        nodePrefab = Resources.Load<GameObject>("SmallNodePrefab");
    }

    // póki co dostępny tylko jeden rodzaj drogi -o-o-o-o-
    public Road Create(Junction from, Junction to)
    {
        if (from == null || to == null)
        {
            Debug.Log("Could not create road, junction is missing.");
            return null;
        }

        Road newRoad = Instantiate(prefab, (from.transform.position + to.transform.position) / 2, Quaternion.identity, transform).GetComponent<Road>();

        newRoad.nodes = new List<Node>();

        VertexPath vertexPath = CreateVertexPath(new Vector3[] { from.transform.position, newRoad.transform.position, to.transform.position });

        BuildPlaceholderTypeRoad(newRoad, vertexPath);

        from.exits.Add(newRoad);
        to.entries.Add(newRoad);

        foreach (var entryNode in from.entries.Select(e => e.endNode))
        {
            VertexPath path = CreateVertexPath(new Vector3[] { entryNode.transform.position, from.transform.position, newRoad.startNode.transform.position });
            entryNode.consequent.Add(newRoad.startNode, path);
        }

        foreach (var exitNode in to.exits.Select(e => e.startNode))
        {
            VertexPath path = CreateVertexPath(new Vector3[] { newRoad.endNode.transform.position, to.transform.position, exitNode.transform.position });
            newRoad.endNode.consequent.Add(exitNode, path);
        }

        roads.Add(newRoad);

        return newRoad;
    }

    public void Delete(Road road)
    {
        roads.Remove(road);
        DestroyImmediate(road.gameObject);
    }


    VertexPath CreateVertexPath(Vector3[] points)
    {
        BezierPath bezierPath = new BezierPath(points, isClosed: false, PathSpace.xy);
        return new VertexPath(bezierPath);
    }

    void BuildPlaceholderTypeRoad(Road road, VertexPath vertexPath)
    {
        var startDistance = 0.2f;
        var step = 0.2f;

        var currentNode = Instantiate(nodePrefab, vertexPath.GetPoint(startDistance, EndOfPathInstruction.Stop), Quaternion.identity, road.transform);
        road.nodes.Add(currentNode.GetComponent<Node>());

        for (var i = startDistance; i <= 1 - startDistance; i += step)
        {
            Vector3[] pathSegment = new Vector3[3];
            pathSegment[0] = vertexPath.GetPoint(i, EndOfPathInstruction.Stop); //start
            pathSegment[1] = vertexPath.GetPoint(i + step / 2, EndOfPathInstruction.Stop); //middle
            pathSegment[2] = vertexPath.GetPoint(i + step, EndOfPathInstruction.Stop); //end

            var node = Instantiate(nodePrefab, pathSegment[2], Quaternion.identity, road.transform);

            var segmentVertexPath = CreateVertexPath(pathSegment);
            currentNode.GetComponent<Node>().consequent.Add(node.GetComponent<Node>(), segmentVertexPath);
            currentNode = node;

            road.nodes.Add(currentNode.GetComponent<Node>());
        }

        road.startNode = road.nodes.FirstOrDefault();
        road.endNode = road.nodes.LastOrDefault();
    }
}
