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

    public void AddConsequent(ISelectable successor)
    {
        Junction nextJunction = successor as Junction;
        if (nextJunction != null)
        {
            Road road = SimulationManager.RoadManager.Create(this, nextJunction, 3, 4f, 0.1f);
            if (road != null)
                consequent.Add(new InterjunctionConnection(nextJunction, road));
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
