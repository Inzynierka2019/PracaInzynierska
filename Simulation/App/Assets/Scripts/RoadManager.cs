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

    public Road Create(Junction from, Junction to, int laneCount, float laneWidth, float nodeDensity)
    {
        if (from == null || to == null)
        {
            Debug.Log("Could not create road, junction is missing.");
            return null;
        }

        Road newRoad = Instantiate(prefab, (from.transform.position + to.transform.position) / 2, Quaternion.identity, transform).GetComponent<Road>();

        newRoad.nodes = new List<Node[]>();

        List<VertexPath> vertexPaths = new List<VertexPath>();
        Vector3 laneSeparation = laneWidth * Vector2.Perpendicular(to.transform.position - from.transform.position);
        for(int i = 0; i < laneCount; i++)
        {
            Vector3 offset = (0.5f * (laneCount - 1) - i) * laneSeparation;
            vertexPaths.Add(CreateVertexPath(new Vector3[] { from.transform.position + offset, newRoad.transform.position + offset, to.transform.position + offset}));
        }

        float stepLength = 1.0f / (nodeDensity * Vector3.Distance(from.transform.position, to.transform.position));

        Node[] currentStepNodes = vertexPaths.Select(vp => Instantiate(nodePrefab, vp.GetPoint(stepLength, EndOfPathInstruction.Stop), Quaternion.identity, newRoad.transform).GetComponent<Node>()).ToArray();
        newRoad.nodes.Add(currentStepNodes);

        for (float i = stepLength; i <= 1 - stepLength; i += stepLength)
        {
            Node[] nextStepNodes = new Node[laneCount];

            for (int l = 0; l < laneCount; l++)
            {
                Vector3[] pathSegment = new Vector3[3];
                pathSegment[0] = vertexPaths[l].GetPoint(i, EndOfPathInstruction.Stop); //start
                pathSegment[1] = vertexPaths[l].GetPoint(i + stepLength / 2, EndOfPathInstruction.Stop); //middle
                pathSegment[2] = vertexPaths[l].GetPoint(i + stepLength, EndOfPathInstruction.Stop); //end

                Node node = Instantiate(nodePrefab, pathSegment[2], Quaternion.identity, newRoad.transform).GetComponent<Node>();

                currentStepNodes[l].consequent.Add(node, CreateVertexPath(pathSegment));

                if(l > 0)
                {
                    pathSegment[0] = vertexPaths[l - 1].GetPoint(i, EndOfPathInstruction.Stop);
                    pathSegment[1] = (pathSegment[0] + pathSegment[2]) / 2;
                    currentStepNodes[l - 1].consequent.Add(node, CreateVertexPath(pathSegment));
                }

                if (l < laneCount - 1)
                {
                    pathSegment[0] = vertexPaths[l + 1].GetPoint(i, EndOfPathInstruction.Stop);
                    pathSegment[1] = (pathSegment[0] + pathSegment[2]) / 2;
                    currentStepNodes[l + 1].consequent.Add(node, CreateVertexPath(pathSegment));
                }

                nextStepNodes[l] = node;
            }

            currentStepNodes = nextStepNodes;
            newRoad.nodes.Add(currentStepNodes);
        }

        newRoad.startNodes = newRoad.nodes.FirstOrDefault();
        newRoad.endNodes = newRoad.nodes.LastOrDefault();

        from.exits.Add(newRoad);
        to.entries.Add(newRoad);

        if(from.entries.Count > 0)
        {
            foreach (Node entryNode in from.entries.Select(e => e.endNodes).Aggregate((a1, a2) => a1.Concat(a2).ToArray()))
            {
                foreach (Node n in newRoad.startNodes)
                {
                    VertexPath path = CreateVertexPath(new Vector3[] { entryNode.transform.position, from.transform.position, n.transform.position });
                    entryNode.consequent.Add(n, path);
                }
            }
        }

        if(to.exits.Count > 0)
        {
            foreach (Node exitNode in to.exits.Select(e => e.startNodes).Aggregate((a1, a2) => a1.Concat(a2).ToArray()))
            {
                foreach (Node n in newRoad.endNodes)
                {
                    VertexPath path = CreateVertexPath(new Vector3[] { n.transform.position, to.transform.position, exitNode.transform.position });
                    n.consequent.Add(exitNode, path);
                }
            }
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
}
