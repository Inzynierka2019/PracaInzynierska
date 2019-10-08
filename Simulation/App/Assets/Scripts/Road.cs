using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [HideInInspector]
    public List<Node> nodes;

    [HideInInspector]
    public Node startNode;
    [HideInInspector]
    public Node endNode;

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
}
