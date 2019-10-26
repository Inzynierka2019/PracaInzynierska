using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        //var cache = new Dictionary<Junction, List<Junction>>();

        //foreach (Junction j in junctions)
        //{
        //    j.ClearConnectionsAndPaths();
        //    cache.Add(j, j.consequent.Select(c => c.junction).ToList());
        //    j.consequent = new List<Junction.InterjunctionConnection>();
        //}

        //foreach (Junction j in junctions)
        //{
        //    List<Junction> nodeNeighbours = cache[j];
        //    foreach (var neighbour in nodeNeighbours)
        //    {
        //        j.AddConsequent(neighbour);
        //    }
        //}

        List<Road> existingRoads = new List<Road>();

        foreach (Junction j in junctions)
        {
            foreach (Road r in j.exits.ToList())
            {
                Junction target = j.consequent.FirstOrDefault(c => c.road == r).junction;
                if (target != null)
                {
                    existingRoads.Add(r);
                    SimulationManager.RoadManager.RebuildNodes(r, j, target, 4f, 0.1f);
                }
                else
                {
                    j.exits.Remove(r);
                }
            }
        }

        foreach (Road r in SimulationManager.RoadManager.roads.ToList())
        {
            if (!existingRoads.Contains(r))
            {
                SimulationManager.RoadManager.Delete(r);
            }
        }

        foreach (Junction j in junctions)
        {
            j.entries.RemoveAll(r => r == null);
        }
    }
}
