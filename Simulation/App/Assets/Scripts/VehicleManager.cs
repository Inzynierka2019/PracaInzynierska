using Common.Models;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VehicleManager : MonoBehaviour
{
    public List<Vehicle> vehicles;
    public Dictionary<Personality, int> driverProfiles;
    GameObject prefab;

    void Start()
    {
        vehicles = new List<Vehicle>();
        prefab = Resources.Load<GameObject>("VehiclePrefab");
        driverProfiles = new Dictionary<Personality, int>()
        {
            { Personality.Slow, SimulationManager.ScenePreference.slowDriverSpawnChance },
            { Personality.Normal, SimulationManager.ScenePreference.normalDriverSpawnChance },
            { Personality.Aggresive, SimulationManager.ScenePreference.aggresiveDriverSpawnChance }
        };
    }

    public Vehicle Create(Node spawnPoint, Node target, string roadTypeName)
    {
        if (spawnPoint == null || target == null)
        {
            Debug.Log("Could not create vehicle, node is missing.");
            return null;
        }

        Vehicle newVehicle = Instantiate(prefab, spawnPoint.transform.position, Quaternion.identity, transform).GetComponent<Vehicle>();
        newVehicle.roadTypeName = roadTypeName;
        newVehicle.driver = CreateDriverProfile();
        newVehicle.CalculateBestPath(spawnPoint.consequent[0].node, target);
        spawnPoint.vehicles.Add(newVehicle);

        vehicles.Add(newVehicle);

        return newVehicle;
    }

    public Driver CreateDriverProfile()
    {
        float threshold = Random.Range(0, driverProfiles.Values.Sum());
        foreach (var profile in driverProfiles)
        {
            threshold -= profile.Value;
            if (threshold < 0)
            {
                return GetDriverProfile(profile.Key) ??
                    throw new System.Exception(
                        "Personality doesn't match with the key in dictionary in CreateDriverProfile()");
            }
        }
        // fallback
        return new NormalDriver();
    }

    private Driver GetDriverProfile(Personality personality)
    {
        switch(personality)
        {
            case Personality.Slow:
                return new SlowDriver();
            case Personality.Normal:
                return new NormalDriver();
            case Personality.Aggresive:
                return new AggresiveDriver();
        }
        return null;
    }

    public void Delete(Vehicle vehicle)
    {
        vehicles.Remove(vehicle);
        DestroyImmediate(vehicle.gameObject);
    }
}

/* 
 aggresive: 30%
 normal: 50%
 slow: 20%
     
     */
