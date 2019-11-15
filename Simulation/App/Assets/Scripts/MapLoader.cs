using Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapLoader : MonoBehaviour
{
    void Start()
    {
        var configName = "simulation-preferences.json";
        var configPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, configName);

        Debug.Log($"Config path (loader): {configPath}");

        using (var reader = new StreamReader(configPath))
        {
            try
            {
                var json = reader.ReadToEnd();
                var simulationPreferences = JsonConvert.DeserializeObject<SimulationPreferences>(json);
                SceneManager.LoadScene(simulationPreferences.currentScene);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }
    }
}
