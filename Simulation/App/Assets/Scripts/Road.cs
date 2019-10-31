using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Road : MonoBehaviour
{
    //[HideInInspector]
    // lista kolekcji się nie serializuje - unity nie zapamiętuje stanu w scenie takich obiektów
    public List<NodeArray> nodes;

    [Serializable]
    public struct NodeArray : IEnumerable
    {
        public Node[] array;

        public IEnumerator GetEnumerator()
        {
            return array.GetEnumerator();
        }

        public static implicit operator Node[](NodeArray na)
        {
            return na.array;
        }

        public static implicit operator NodeArray(Node[] a)
        {
            NodeArray na = new NodeArray();
            na.array = a;
            return na;
        }
    }

    [HideInInspector]
    public Node[] startNodes;
    [HideInInspector]
    public Node[] endNodes;

    [HideInInspector]
    public float offsetToRight;
    [HideInInspector]
    public float pathWeight;

    void OnDrawGizmos()
    {
        if(nodes != null)
        {
            foreach (Node node in nodes.Aggregate((a1, a2) => a1.array.Concat(a2.array).ToArray()))
            {
                foreach (var path in node.consequent.Select(c => c.path))
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
