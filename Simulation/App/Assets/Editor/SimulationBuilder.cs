using UnityEditor;
using System.IO;

class SimulationBuilder
{
    private static string[] GetScenes()
    {
        return new string[] {
            "Assets/Scenes/MapLoader.unity",
            "Assets/Scenes/Wrzeszcz.unity",
            "Assets/Scenes/Manhattan.unity",
        };
    }

    static void BuildWindows()
    {
        var pathToDeploy = Path.Combine(Directory.GetCurrentDirectory(), "out\\Windows\\App.exe");

        BuildPipeline.BuildPlayer(GetScenes(), pathToDeploy, BuildTarget.StandaloneWindows64, BuildOptions.ShowBuiltPlayer);
    }

    static void BuildWebGL()
    {
        var pathToDeploy = Path.Combine(Directory.GetCurrentDirectory(), "out\\WebGL\\");

        BuildPipeline.BuildPlayer(GetScenes(), pathToDeploy, BuildTarget.WebGL, BuildOptions.ShowBuiltPlayer);
    }
}