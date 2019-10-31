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
                if (e.button == 2)
                {
                    HandleUtility.AddDefaultControl(controlID);
                }
                else
                {
                    GUIUtility.hotControl = controlID;
                    e.Use();
                }
                break;
            case EventType.MouseUp:
                GUIUtility.hotControl = 0;
                switch (e.button)
                {
                    case 0:
                        HandleClick();
                        break;
                }
                e.Use();
                break;
            case EventType.MouseDrag:
                HandleUtility.AddDefaultControl(controlID);
                //GUIUtility.hotControl = controlID;
                //e.Use();
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
            node.GetComponent<ISelectable>().Mark(select);

            if (select)
                selectedNode = node.gameObject;
            else
                selectedNode = null;

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
                var node = hitInfo.transform.GetComponent<Node>();

                if (!isLeftCtrPressed)
                {
                    if (junction == null)
                    {
                        if (node == null)
                            EditorUtility.SetDirty(SimulationManager.JunctionManager.Create(new Vector3(hitInfo.point.x, hitInfo.point.y), (selectedNode != null) ? selectedNode.GetComponent<Junction>() : null));
                        else
                            ChangeSelection(node.gameObject);
                    }
                    else
                    {
                        ChangeSelection(junction.gameObject);
                    }
                }
                else if (selectedNode != null)
                {
                    if (junction != null)
                    {
                        if (SimulationManager.RoadManager.backwardLaneCountSetting > 0)
                            selectedNode.GetComponent<Junction>()?.AddConsequentBothWays(junction);
                        else
                            selectedNode.GetComponent<Junction>()?.AddConsequent(junction);

                    }
                    else if (node != null)
                    {
                        selectedNode.GetComponent<Node>()?.AddConsequent(node);
                    }
                }
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

        GUIStyle richtextStyle = new GUIStyle() { richText = true };

        GUILayout.Space(20);

        GUILayout.Label("<b>Global road options:</b>", richtextStyle);
        SimulationManager.RoadManager.LaneWidth = EditorGUILayout.Slider("Lane width:", SimulationManager.RoadManager.LaneWidth, 1f, 10f);
        SimulationManager.RoadManager.NodeDensity = EditorGUILayout.Slider("Node density:", SimulationManager.RoadManager.NodeDensity, 0.001f, 0.5f);

        GUILayout.Space(20);

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

        GUILayout.Space(20);

        GUILayout.Label("<b>Next road options:</b>", richtextStyle);
        SimulationManager.RoadManager.laneCountSetting = EditorGUILayout.IntSlider("Number of lanes:", SimulationManager.RoadManager.laneCountSetting, 1, 6);
        SimulationManager.RoadManager.backwardLaneCountSetting = EditorGUILayout.IntSlider("Number of opposite lanes:", SimulationManager.RoadManager.backwardLaneCountSetting, 0, 6);
        SimulationManager.RoadManager.distanceBetweenLanesSetting = EditorGUILayout.Slider("Distance between lanes:", SimulationManager.RoadManager.distanceBetweenLanesSetting, 0f, 3f);

        GUILayout.Space(10);

        SimulationManager.RoadManager.pathWeightSetting = EditorGUILayout.Slider("Road attractiveness:", SimulationManager.RoadManager.pathWeightSetting, 0.001f, 2.0f);

        GUILayout.Space(10);

        GUILayout.Label("Road spawn weights:");
        for (int i = 0; i < SpawnManager.spawnTypeCount; i++)
        {
            SimulationManager.RoadManager.spawnWeightsSetting[i] = EditorGUILayout.Slider($"{SpawnManager.spawnTypes[i]}:", SimulationManager.RoadManager.spawnWeightsSetting[i], 0f, 1f);
        }

        GUILayout.Space(20);
    }
}

#endif