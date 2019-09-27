using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    void Start()
    {
        RecreatePaths();
    }

    // ten system totalnie absolutnie do zmiany
    public enum Containers
    {
        BigNodesContainer,
        SmallNodesContainer
    }

    public static GameObject GetNodesContainer(Containers container)
    {
        var nodesContainer = GameObject.Find(container.ToString());
        if (nodesContainer == null)
        {
            nodesContainer = new GameObject();
            nodesContainer.name = container.ToString();
        }
        return nodesContainer;
    }

    public static void RecreatePaths()
    {
        Debug.Log("Recreating paths. Please hold");
        var smallNodes = GetNodesContainer(Containers.SmallNodesContainer);
        GameObject.DestroyImmediate(smallNodes);

        var bigNodes = GetNodesContainer(Containers.BigNodesContainer);
        var cache = new Dictionary<Junction, List<Junction>>();
        foreach (Transform node in bigNodes.transform)
        {
            var nodeComponent = node.GetComponent<Junction>();
            nodeComponent.ClearConnectionsAndPaths();
            cache.Add(nodeComponent, nodeComponent.consequent);
            nodeComponent.consequent = new List<Junction>();
        }

        foreach (Transform node in bigNodes.transform)
        {
            var nodeComponent = node.GetComponent<Junction>();
            var nodeNeighbours = cache[nodeComponent];
            foreach (var neighbour in nodeNeighbours)
                nodeComponent.AddConsequent(neighbour);
        }
        Debug.Log("Recreating paths. Done.");
    }
}
