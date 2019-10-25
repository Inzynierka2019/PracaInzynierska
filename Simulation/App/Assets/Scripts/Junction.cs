using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
    
    public void Start()
    {
        StartCoroutine(TrafficLightsControlerCoroutine());
    }

    void Update()
    {
        if (!Application.isPlaying && transform.hasChanged)
        {
            SimulationManager.JunctionManager.RebuildRoads();
            transform.hasChanged = false;
        }
    }

    // Works only if Application.isPlaying
    void OnMouseDown()
    {
        Road sourceRoad = exits[Random.Range(0, exits.Count)];
        Node sourceNode = sourceRoad.startNodes[Random.Range(0, sourceRoad.startNodes.Length)];
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
        if (SimulationManager.RoadManager.Create(this, successor, 3, 0.02f, 0.1f) != null)
            consequent.Add(successor);
    }

    private IEnumerator TrafficLightsControlerCoroutine()
    {
        if (Application.isPlaying)
        {
            List<Node> allEntryNodes = entries.Select(r => r.endNodes).Aggregate((a1, a2) => a1.Concat(a2).ToArray()).ToList();

            while (Application.isPlaying)
            {
                allEntryNodes.ForEach(n => n.ChangeLightsToRed());
                allEntryNodes.ElementAt(Random.Range(0, allEntryNodes.Count)).ChangeLightsToGreen();

                yield return new WaitForSeconds(3.5f);
            }
        }
    }
}
