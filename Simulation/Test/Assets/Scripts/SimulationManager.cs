using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    //private void OnDrawGizmos()
    //{
    //    var container = ObjectBuilderEditor.GetNodesContainer(ObjectBuilderEditor.Containers.SmallNodesContainer);

    //    foreach (Transform child in container.transform)
    //    {
    //        foreach (var path in child.GetComponent<Node>().paths)
    //        {
    //            //Gizmos.DrawLine(path.Value.vertices[0], path.Value.vertices[path.Value.vertices.Length - 1]);
    //            for (var i = 1; i < path.Value.vertices.Length; i++)
    //            {
    //                Gizmos.DrawLine(path.Value.vertices[i - 1], path.Value.vertices[i]);
    //            }

    //            Gizmos.DrawSphere(path.Value.GetPointAtDistance(path.Value.length - 0.15f, PathCreation.EndOfPathInstruction.Stop), 0.08f);
    //        }
    //    }
    //}
    // Start is called before the first frame update
    void Start()
    {
        ObjectBuilderEditor.RecreatePaths();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
