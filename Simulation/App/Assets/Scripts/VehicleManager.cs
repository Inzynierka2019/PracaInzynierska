using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VehicleManager : MonoBehaviour
{
    //List<Road> roads;

    //GameObject prefab, nodePrefab;

    //void Awake()
    //{
    //    roads = new List<Road>(GetComponentsInChildren<Road>());

    //    prefab = Resources.Load<GameObject>("RoadPrefab");
    //    nodePrefab = Resources.Load<GameObject>("SmallNodePrefab");
    //}

    //// póki co dostępny tylko jeden rodzaj drogi -o-o-o-o-
    //public Road Create(Junction from, Junction to)
    //{
    //    if (from == null || to == null)
    //    {
    //        Debug.Log("Could not create road, junction is missing.");
    //        return null;
    //    }

    //    Road newRoad = Instantiate(prefab, (from.transform.position + to.transform.position) / 2, Quaternion.identity, transform).GetComponent<Road>();

    //    newRoad.nodes = new List<Node>();

    //    VertexPath vertexPath = CreateVertexPath(new Vector3[] { from.transform.position, newRoad.transform.position, to.transform.position });

    //    BuildPlaceholderTypeRoad(newRoad, vertexPath);

    //    from.exits.Add(newRoad);
    //    to.entries.Add(newRoad);

    //    foreach (var entryNode in from.entries.Select(e => e.endNode))
    //    {
    //        VertexPath path = CreateVertexPath(new Vector3[] { entryNode.transform.position, from.transform.position, newRoad.startNode.transform.position });
    //        entryNode.consequent.Add(newRoad.startNode, path);
    //    }

    //    foreach (var exitNode in to.exits.Select(e => e.startNode))
    //    {
    //        VertexPath path = CreateVertexPath(new Vector3[] { newRoad.endNode.transform.position, to.transform.position, exitNode.transform.position });
    //        newRoad.endNode.consequent.Add(exitNode, path);
    //    }

    //    return newRoad;
    //}
}
