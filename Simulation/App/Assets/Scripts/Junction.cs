using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class Junction : MonoBehaviour, ISelectable
{
    [SerializeField] Material idleMat;
    [SerializeField] Material selectedMat;

    //[HideInInspector]
    public List<Road> entries = new List<Road>();
    //[HideInInspector]
    public List<Road> exits = new List<Road>();

    // ani dictionary ani tuple nie są serializowalne - unity nie zapamiętuje stanu w scenie takich obiektów
    public List<InterjunctionConnection> consequent = new List<InterjunctionConnection>();

    [Serializable]
    public struct InterjunctionConnection
    {
        public Junction junction;
        public Road road;

        public InterjunctionConnection(Junction j, Road r)
        {
            junction = j;
            road = r;
        }
    }

    public void Start()
    {
        StartCoroutine(TrafficLightsControlerCoroutine());
        transform.hasChanged = false;
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
        SimulationManager.VehicleManager.Create(sourceNode, sourceNode.consequent.Select(c => c.node).ToList()[Random.Range(0, sourceNode.consequent.Count)]);
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
        if (successor != null)
        {
            Road road = SimulationManager.RoadManager.Create(this, successor);
            if (road != null)
                consequent.Add(new InterjunctionConnection(successor, road));
        }
    }

    public void AddConsequentBothWays(Junction neighbour)
    {
        if (neighbour != null)
        {
            Road road = SimulationManager.RoadManager.Create(this, neighbour, backwardLaneCount: false, offseted: true);
            if (road != null)
                consequent.Add(new InterjunctionConnection(neighbour, road));

            road = SimulationManager.RoadManager.Create(neighbour, this, backwardLaneCount: true, offseted: true);
            if (road != null)
                neighbour.consequent.Add(new InterjunctionConnection(this, road));
        }
    }

    public void Mark(bool selected)
    {
        if (selected)
            GetComponent<Renderer>().material = selectedMat;
        else
            GetComponent<Renderer>().material = idleMat;
    }

    private IEnumerator TrafficLightsControlerCoroutine()
    {
        if (entries.Count > 0)
        {
            int entryRoadIndex = 0;
            while (Application.isPlaying)
            {
                entries.Select(r => r.endNodes).ToList().ForEach(n => n.ToList().ForEach(k => k.ChangeLightsToRed()));
                entries[entryRoadIndex].endNodes.ToList().ForEach(n => n.ChangeLightsToGreen());

                entryRoadIndex = (entryRoadIndex + 1) % entries.Count();

                yield return new WaitForSeconds(5f);
            }
        }
    }
}
