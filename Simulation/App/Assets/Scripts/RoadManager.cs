using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using PathCreation;

[ExecuteInEditMode]
public class RoadManager : MonoBehaviour
{
    [HideInInspector]
    public List<Road> roads;

    // serializeField is for unity to remember the value on a way from editor to runtime
    [HideInInspector] [SerializeField]
    float laneWidth = 4f;
    [HideInInspector]
    public float LaneWidth
    {
        get => laneWidth;
        set
        {
            if (laneWidth != value)
            {
                laneWidth = value;
                SimulationManager.Rebuild();
            }
        }
    }

    [HideInInspector] [SerializeField]
    float nodeDensity = 0.1f;
    [HideInInspector]
    public float NodeDensity
    {
        get => nodeDensity;
        set
        {
            if (nodeDensity != value)
            {
                nodeDensity = value;
                SimulationManager.Rebuild();
            }
        }
    }

    [HideInInspector] public int laneCountSetting = 2;
    [HideInInspector] public int backwardLaneCountSetting = 2;
    [HideInInspector] public float distanceBetweenLanesSetting = 0f;

    GameObject prefab, nodePrefab;

    void Awake()
    {
        roads = new List<Road>(GetComponentsInChildren<Road>());

        prefab = Resources.Load<GameObject>("RoadPrefab");
        nodePrefab = Resources.Load<GameObject>("SmallNodePrefab");
    }

    public Road Create(Junction from, Junction to)
    {
        if (from == null || to == null)
        {
            Debug.Log("Could not create road, junction is missing.");
            return null;
        }

        Road newRoad = Instantiate(prefab, (from.transform.position + to.transform.position) / 2, Quaternion.identity, transform).GetComponent<Road>();

        BuildNodes(newRoad, from, to, laneCountSetting);

        from.exits.Add(newRoad);
        to.entries.Add(newRoad);

        // połącz każdy pas z każdym innym pasem na skrzyżowaniach
        //if (from.entries.Count > 0)
        //{
        //    foreach (Node entryNode in from.entries.Select(e => e.endNodes).Aggregate((a1, a2) => a1.Concat(a2).ToArray()))
        //    {
        //        foreach (Node n in newRoad.startNodes)
        //        {
        //            VertexPath path = CreateVertexPath(new Vector3[] { entryNode.transform.position, from.transform.position, n.transform.position });
        //            entryNode.consequent.Add(new Node.InternodeConnection(n, path));
        //        }
        //    }
        //}

        //if (to.exits.Count > 0)
        //{
        //    foreach (Node exitNode in to.exits.Select(e => e.startNodes).Aggregate((a1, a2) => a1.Concat(a2).ToArray()))
        //    {
        //        foreach (Node n in newRoad.endNodes)
        //        {
        //            VertexPath path = CreateVertexPath(new Vector3[] { n.transform.position, to.transform.position, exitNode.transform.position });
        //            n.consequent.Add(new Node.InternodeConnection(exitNode, path));
        //        }
        //    }
        //}

        roads.Add(newRoad);

        return newRoad;
    }

    public void RebuildNodes(Road road, Junction from, Junction to)
    {
        if (from == null || to == null)
        {
            Debug.Log("Could not rebuild road, junction is missing.");
            Delete(road);
            return;
        }

        int laneCount = road.endNodes.Length;

        road.transform.position = (from.transform.position + to.transform.position) / 2;

        List<Node>[] targetNodesCache = road.endNodes.Select(n => n.consequent.Select(c => c.node).Where(node => node != null).ToList()).ToArray();

        if(road.nodes.Count > 1)
        {
            foreach (GameObject node in road.nodes.Skip(1).Aggregate((a1, a2) => a1.array.Concat(a2.array).ToArray()).array.Select(n => n.gameObject).ToList())
            {
                DestroyImmediate(node);
            }
        }
        foreach (Node node in road.startNodes)
        {
            node.consequent.Clear();
        }

        BuildNodes(road, from, to, laneCount);

        for (int i = 0; i < laneCount; i++)
        {
            foreach (Node targetNode in targetNodesCache[i])
            {
                Vector3 midPoint = (road.endNodes[i].transform.position + targetNode.transform.position + to.transform.position) / 3;
                road.endNodes[i].consequent.Add(new Node.InternodeConnection(targetNode, CreateVertexPath(new Vector3[] { road.endNodes[i].transform.position, midPoint, targetNode.transform.position })));
            }
        }
    }

    public void Delete(Road road)
    {
        roads.Remove(road);
        DestroyImmediate(road.gameObject);
    }

    void BuildNodes(Road road, Junction from, Junction to, int laneCount)
    {
        road.nodes = new List<Road.NodeArray>();

        List<VertexPath> vertexPaths = new List<VertexPath>();
        Vector3 laneSeparation = laneWidth * Vector2.Perpendicular(to.transform.position - from.transform.position).normalized;
        for (int i = 0; i < laneCount; i++)
        {
            Vector3 offset = (0.5f * (laneCount - 1) - i) * laneSeparation;
            vertexPaths.Add(CreateVertexPath(new Vector3[] { from.transform.position + offset, road.transform.position + offset, to.transform.position + offset }));
        }

        float nodeCount = nodeDensity * Vector3.Distance(from.transform.position, to.transform.position);
        float firstNodesOffset = 1f / nodeCount;
        float stepLength = (1f - 2 * firstNodesOffset) / (int)(nodeCount - 1);

        if (nodeCount < 2f)
        {
            firstNodesOffset = 0.5f;
            stepLength = 1f;
        }

        Node[] currentStepNodes = road.startNodes;
        if (currentStepNodes == null || currentStepNodes.Length <= 0)
        {
            currentStepNodes = vertexPaths.Select(vp => Instantiate(nodePrefab, vp.GetPoint(firstNodesOffset, EndOfPathInstruction.Stop), Quaternion.identity, road.transform).GetComponent<Node>()).ToArray();
        }
        else
        {
            for (int i = 0; i < laneCount; i++)
                currentStepNodes[i].transform.position = vertexPaths[i].GetPoint(firstNodesOffset, EndOfPathInstruction.Stop);
        }

        road.nodes.Add(currentStepNodes);

        for (float i = firstNodesOffset; i <= 1f - firstNodesOffset - 0.5f * stepLength; i += stepLength)
        {
            Node[] nextStepNodes = new Node[laneCount];

            for (int l = 0; l < laneCount; l++)
            {
                Vector3[] pathSegment = new Vector3[3];
                pathSegment[0] = vertexPaths[l].GetPoint(i, EndOfPathInstruction.Stop); //start
                pathSegment[1] = vertexPaths[l].GetPoint(i + stepLength / 2, EndOfPathInstruction.Stop); //middle
                pathSegment[2] = vertexPaths[l].GetPoint(i + stepLength, EndOfPathInstruction.Stop); //end

                Node node = Instantiate(nodePrefab, pathSegment[2], Quaternion.identity, road.transform).GetComponent<Node>();

                currentStepNodes[l].consequent.Add(new Node.InternodeConnection(node, CreateVertexPath(pathSegment)));

                if (l > 0)
                {
                    pathSegment[0] = vertexPaths[l - 1].GetPoint(i, EndOfPathInstruction.Stop);
                    pathSegment[1] = (pathSegment[0] + pathSegment[2]) / 2;
                    currentStepNodes[l - 1].consequent.Add(new Node.InternodeConnection(node, CreateVertexPath(pathSegment)));
                }

                if (l < laneCount - 1)
                {
                    pathSegment[0] = vertexPaths[l + 1].GetPoint(i, EndOfPathInstruction.Stop);
                    pathSegment[1] = (pathSegment[0] + pathSegment[2]) / 2;
                    currentStepNodes[l + 1].consequent.Add(new Node.InternodeConnection(node, CreateVertexPath(pathSegment)));
                }

                nextStepNodes[l] = node;
            }

            currentStepNodes = nextStepNodes;
            road.nodes.Add(currentStepNodes);
        }

        road.startNodes = road.nodes.FirstOrDefault();
        road.endNodes = road.nodes.LastOrDefault();
    }


    public VertexPath CreateVertexPath(Vector3[] points)
    {
        BezierPath bezierPath = new BezierPath(points, isClosed: false, PathSpace.xy);
        return new VertexPath(bezierPath);
    }
}
