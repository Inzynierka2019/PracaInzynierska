using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[CustomEditor(typeof(ObjectBuilderScript))]
public class ObjectBuilderEditor : Editor
{
    private static bool m_editMode = false;
    private static int m_count = 0;
    public static GameObject selectedNode;
    public bool isLeftCtrPressed = false;

    public enum Containers
    {
        BigNodesContainer,
        SmallNodesContainer
    }

    public enum Tags
    {
        Selected,
        Node,
        Untagged
    }

    public static GameObject GetNodesContainer(Containers container)
    {
        var nodesContainer = GameObject.Find(container.ToString());
        if(nodesContainer == null)
        {
            nodesContainer = new GameObject();
            nodesContainer.name = container.ToString();
        }
        return nodesContainer;
    }

    void OnSceneGUI()
    {
        if (!m_editMode)
            return;
        Event e = Event.current;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        switch (e.GetTypeForControl(controlID))
        {
            case EventType.MouseDown:
                GUIUtility.hotControl = controlID;
                e.Use();
                break;
            case EventType.MouseUp:
                GUIUtility.hotControl = 0;
                Fun();
                e.Use();
                break;
            case EventType.MouseDrag:
                GUIUtility.hotControl = controlID;
                e.Use();
                break;
            case EventType.KeyDown:
                if (e.keyCode == KeyCode.LeftControl)
                    isLeftCtrPressed = true;
                break;
            case EventType.KeyUp:
                if (e.keyCode == KeyCode.LeftControl)
                    isLeftCtrPressed = false;
                break;
        }
    }

    private void Mark(GameObject node, bool select = true)
    {
        if(select)
        {
            node.transform.tag = Tags.Selected.ToString();
            Color newColor = new Color(1, 0, 0, 89 / 255f);
            node.transform.GetComponent<Renderer>().material.color = newColor;
            selectedNode = node.transform.gameObject;
        }
        else
        {
            node.transform.tag = Tags.Node.ToString();
            Color newColor = new Color(0, 0, 1, 89 / 255f);
            node.transform.GetComponent<Renderer>().material.color = newColor;
            selectedNode = null;
        }
    }

    /// <summary>
    /// Zaznacza wezel na czerwono i zwraca true jesli dany wezel został zaznaczony.
    /// </summary>
    private bool ChangeSelection(GameObject node)
    {
        if (node?.transform.tag == Tags.Node.ToString())
        {
            if(selectedNode)
                Mark(selectedNode, select: false);

            Mark(node, select: true);
            return true;
        }
        else if(node?.transform.tag == Tags.Selected.ToString())
        {
            Mark(node, select: false);
            return true;
        }
        return false;
    }

    void Fun()
    {
        if (m_editMode)
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            
            if (Physics.Raycast(worldRay, out RaycastHit hitInfo))
            {
                if (!isLeftCtrPressed)
                {
                    if (ChangeSelection(hitInfo.transform.gameObject))
                        return;

                    var newObject = Instantiate(Resources.Load<GameObject>("NodePrefab"));
                    newObject.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y);
                    newObject.name = "Node" + m_count.ToString("00");

                    selectedNode?.GetComponent<Node>().AddNeighbour(newObject.GetComponent<Node>());

                    EditorUtility.SetDirty(newObject);
                    Transform container = GetNodesContainer(Containers.BigNodesContainer).transform;
                    newObject.transform.parent = container;
                    m_count++;
                }
                else
                    selectedNode?.GetComponent<Node>().AddNeighbour(hitInfo.transform.GetComponent<Node>());
            }
            else
                Debug.Log("Coś poszło nie tak :/");
        }
    }


    public static void RecreatePaths()
    {
        Debug.Log("Recreating paths. Please hold");
        var smallNodes = GetNodesContainer(Containers.SmallNodesContainer);
        GameObject.DestroyImmediate(smallNodes);

        var bigNodes = GetNodesContainer(Containers.BigNodesContainer);
        var cache = new Dictionary<Node, List<Node>>();
        foreach (Transform node in bigNodes.transform)
        {
            var nodeComponent = node.GetComponent<Node>();
            nodeComponent.ClearConnectionsAndPaths();
            cache.Add(nodeComponent, nodeComponent.neighbours);
            nodeComponent.neighbours = new List<Node>();
        }

        foreach (Transform node in bigNodes.transform)
        {
            var nodeComponent = node.GetComponent<Node>();
            var nodeNeighbours = cache[nodeComponent];
            foreach (var neighbour in nodeNeighbours)
                nodeComponent.AddNeighbour(neighbour);
        }
        Debug.Log("Recreating paths. Done.");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (m_editMode)
        {
            if (GUILayout.Button("Disable Editing"))
            {
                m_editMode = false;
            }
        }
        else
        {
            if (GUILayout.Button("Enable Editing"))
            {
                m_editMode = true;
                ChangeSelection(selectedNode);
            }
        }

        if(GUILayout.Button("Clean road editor memory"))
        {
            selectedNode = null;
        }

        if(GUILayout.Button("Re-create paths"))
        {
            RecreatePaths();
        }
    }
}

