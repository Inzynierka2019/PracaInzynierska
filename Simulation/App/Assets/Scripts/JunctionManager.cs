using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class JunctionManager : MonoBehaviour
{
    List<Junction> junctions;

    GameObject prefab;

    void Awake()
    {
        junctions = new List<Junction>(GetComponentsInChildren<Junction>());

        prefab = Resources.Load<GameObject>("JunctionPrefab");
    }

    public Junction Create(Vector3 position, Junction predecessor = null)
    {
        Junction newJunction = Instantiate(prefab, position, Quaternion.identity, transform).GetComponent<Junction>();

        predecessor?.AddConsequent(newJunction);

        junctions.Add(newJunction);

        return newJunction;
    }

    public void Delete(Junction junction)
    {
        junctions.Remove(junction);
        DestroyImmediate(junction.gameObject);
        RebuildRoads();
    }

    public void RebuildRoads()
    {
        var cache = new Dictionary<Junction, List<Junction>>();

        foreach (Junction j in junctions)
        {
            j.ClearConnectionsAndPaths();
            cache.Add(j, j.consequent);
            j.consequent = new List<Junction>();
        }

        foreach (Junction j in junctions)
        {
            var nodeNeighbours = cache[j];
            foreach (var neighbour in nodeNeighbours)
                j.AddConsequent(neighbour);
        }
    }
}
