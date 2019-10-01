using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if(UNITY_EDITOR)

[CustomEditor(typeof(ObjectBuilderScript))]
public class ObjectBuilderEditor : Editor
{
    private static bool editMode = true;
    public static GameObject selectedNode;
    public bool isLeftCtrPressed = false;

    void OnSceneGUI()
    {
        if (!editMode)
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
                HandleClick();
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
                switch (e.keyCode)
                {
                    case KeyCode.LeftControl:
                        isLeftCtrPressed = false;
                        break;
                    case KeyCode.D:
                        HandleDelete();
                        break;
                }
                break;
        }
    }

    private void Mark(GameObject node, bool select = true)
    {
        if (node != null)
        {
            if (select)
            {
                Color newColor = new Color(1, 0, 0, 89 / 255f);
                node.transform.GetComponent<Renderer>().material.color = newColor;
                selectedNode = node.gameObject;
            }
            else
            {
                Color newColor = new Color(0, 0, 1, 89 / 255f);
                node.transform.GetComponent<Renderer>().material.color = newColor;
                selectedNode = null;
            }

            EditorUtility.SetDirty(node);
        }
    }

    private void ChangeSelection(GameObject node)
    {
        if (node != selectedNode)
        {
            Mark(selectedNode, select: false);
            Mark(node, select: true);
        }
        else
        {
            Mark(node, select: false);
        }
    }

    void HandleClick()
    {
        if (editMode)
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            if (Physics.Raycast(worldRay, out RaycastHit hitInfo))
            {
                var junction = hitInfo.transform.GetComponent<Junction>();

                if (!isLeftCtrPressed)
                {
                    if (junction == null)
                    {
                        EditorUtility.SetDirty(SimulationManager.JunctionManager.Create(new Vector3(hitInfo.point.x, hitInfo.point.y), selectedNode?.GetComponent<Junction>()));
                    }
                    else
                    {
                        ChangeSelection(junction.gameObject);
                    }

                }
                else
                    selectedNode?.GetComponent<Junction>().AddConsequent(junction);
            }
            else
                Debug.Log("Coś poszło nie tak :/");
        }
    }

    void HandleDelete()
    {
        if (editMode)
        {
            if (selectedNode != null)
            {
                SimulationManager.JunctionManager.Delete(selectedNode.GetComponent<Junction>());
                selectedNode = null;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (editMode)
        {
            if (GUILayout.Button("Disable Editing"))
            {
                editMode = false;
                Mark(selectedNode, select: false);
            }
        }
        else
        {
            if (GUILayout.Button("Enable Editing"))
            {
                editMode = true;
            }
        }

        if (GUILayout.Button("Rebuild"))
        {
            SimulationManager.Rebuild();
        }
    }
}

#endif