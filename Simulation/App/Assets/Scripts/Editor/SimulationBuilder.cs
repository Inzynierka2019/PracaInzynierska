using UnityEditor;
using UnityEngine;

public class SimulationBuilder
{
    static string[] scenes = {
        "Assets/Scenes/ExampleScene.unity"
    };

    const string projectName = "MyGame";
    const string buildPath = "./out/" + projectName;
    
    [MenuItem("Build/Build Windows")]
    static void Build()
    {
        BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.StandaloneWindows64, BuildOptions.None);
    }
}