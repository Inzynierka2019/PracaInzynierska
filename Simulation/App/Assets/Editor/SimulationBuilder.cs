using UnityEditor;

class SimulationBuilder {

    private static string[] GetScenes()
    {
        return new string[] {
            "Assets/Scenes/CitySimulation.unity",
        };
    }

    static void BuildWindows()
    {
        string pathToDeploy = @"D:\Dokumenty\Inzynierka\PracaInzynierska\Simulation\App\out\Simulation.exe";

        BuildPipeline.BuildPlayer(GetScenes(), pathToDeploy, BuildTarget.StandaloneWindows64, BuildOptions.ShowBuiltPlayer);
    }

    static void BuildWebGL()
    {
        string pathToDeploy = @"D:\Dokumenty\Inzynierka\PracaInzynierska\Simulation\App\out\WebGL\";

        BuildPipeline.BuildPlayer(GetScenes(), pathToDeploy, BuildTarget.WebGL, BuildOptions.ShowBuiltPlayer);
    }
}
