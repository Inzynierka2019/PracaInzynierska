using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEditor;
using Random = UnityEngine.Random;

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

    [Space(20)]

    public float pathWeight;
    public float[] spawnWeights;

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

#if(UNITY_EDITOR)
            Handles.Label(transform.position, spawnWeights.Select(w => w.ToString()).Aggregate((w1, w2) => $"{w1}\n{w2}"));
#endif
        }
    }

    public Node GetRandomNode()
    {
        Node[] step = nodes[Random.Range(0, nodes.Count)];
        return step[Random.Range(0, step.Length)];
    }
}
