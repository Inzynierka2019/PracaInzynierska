using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Road : MonoBehaviour
{
    [HideInInspector]
    public List<Node[]> nodes;

    [HideInInspector]
    public Node[] startNodes;
    [HideInInspector]
    public Node[] endNodes;

    void OnDrawGizmos()
    {
        if(nodes != null)
        {
            foreach (Node node in nodes.Aggregate((a1, a2) => a1.Concat(a2).ToArray()))
            {
                foreach (var path in node.consequent.Values)
                {
                    for (int i = 1; i < path.vertices.Length; i++)
                    {
                        Gizmos.DrawLine(path.vertices[i - 1], path.vertices[i]);
                    }

                    Gizmos.DrawSphere(path.GetPointAtDistance(path.length - 2f, PathCreation.EndOfPathInstruction.Stop), 1f);
                }
            }
        }
    }
}
