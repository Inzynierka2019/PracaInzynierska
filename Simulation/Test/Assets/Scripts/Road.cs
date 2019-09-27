using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using PathCreation;

public class Road : MonoBehaviour
{
    List<Node> nodes;

    [HideInInspector]
    public Node startNode;

    [HideInInspector]
    public Node endNode;

    static int nodeId = 0;

    public void Create(Junction from, Junction to)
    {
        nodes = new List<Node>();

        transform.position = (from.transform.position + to.transform.position) / 2;

        from.consequent.Add(to);

        var vertexPath = CreateVertexPath(
            new Vector3[] {
                from.transform.position,
                transform.position,
                to.transform.position });

        var startDistance = 0.2f;
        var step = 0.2f;

        var currentNode = CreateNodeAtPosition(vertexPath.GetPoint(startDistance, EndOfPathInstruction.Stop));
        nodes.Add(currentNode.GetComponent<Node>());

        for (var i = startDistance; i <= 1 - startDistance; i += step)
        {
            Vector3[] pathSegment = new Vector3[3];
            pathSegment[0] = vertexPath.GetPoint(i, EndOfPathInstruction.Stop); //start
            pathSegment[1] = vertexPath.GetPoint(i + step / 2, EndOfPathInstruction.Stop); //middle
            pathSegment[2] = vertexPath.GetPoint(i + step, EndOfPathInstruction.Stop); //end

            var node = CreateNodeAtPosition(pathSegment[2]);

            var segmentVertexPath = CreateVertexPath(pathSegment);
            currentNode.GetComponent<Node>().consequent.Add(node.GetComponent<Node>(), segmentVertexPath);
            currentNode = node;

            nodes.Add(currentNode.GetComponent<Node>());
        }

        startNode = nodes.FirstOrDefault();
        endNode = nodes.LastOrDefault();

        from.exits.Add(this);
        to.entries.Add(this);

        foreach (var entryNode in from.entries.Select(e => e.endNode))
        {
            VertexPath path = CreateVertexPath(new Vector3[] { entryNode.transform.position, from.transform.position, startNode.transform.position });
            entryNode.consequent.Add(startNode, path);
        }

        foreach (var exitNode in to.exits.Select(e => e.startNode))
        {
            VertexPath path = CreateVertexPath(new Vector3[] { endNode.transform.position, to.transform.position, exitNode.transform.position });
            endNode.consequent.Add(exitNode, path);
        }
    }

    void OnDrawGizmos()
    {
        if(nodes != null)
        {
            foreach (var node in nodes)
            {
                foreach (var path in node.consequent.Values)
                {
                    for (var i = 1; i < path.vertices.Length; i++)
                    {
                        Gizmos.DrawLine(path.vertices[i - 1], path.vertices[i]);
                    }

                    Gizmos.DrawSphere(path.GetPointAtDistance(path.length - 0.15f, PathCreation.EndOfPathInstruction.Stop), 0.08f);
                }
            }
        }
    }


    VertexPath CreateVertexPath(Vector3[] points)
    {
        BezierPath bezierPath = new BezierPath(points, isClosed: false, PathSpace.xy);
        return new VertexPath(bezierPath);
    }

    GameObject CreateNodeAtPosition(Vector3 position)
    {
        var smallNode = Instantiate(Resources.Load<GameObject>("SmallNodePrefab"));
        smallNode.name = (nodeId++).ToString();
        smallNode.transform.position = position;
#if(UNITY_EDITOR)
        smallNode.tag = ObjectBuilderEditor.Tags.Untagged.ToString();
#endif
        smallNode.transform.parent = transform;

        return smallNode;
    }
}
