using Common.Models;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public List<Vehicle> vehicles;
    GameObject prefab;

    void Start()
    {
        vehicles = new List<Vehicle>();
        prefab = Resources.Load<GameObject>("VehiclePrefab");
    }

    public bool Create(Node spawnPoint, Node target, string roadTypeName)
    {
        if (spawnPoint == null || target == null)
        {
            //Debug.Log("Could not create vehicle, node is missing.");
            return false;
        }

        Vehicle newVehicle = Instantiate(prefab, spawnPoint.transform.position, Quaternion.identity, transform).GetComponent<Vehicle>();
        newVehicle.roadTypeName = roadTypeName;
        newVehicle.AssignDriver(DriverFactory.GetRandomDriver());

        try
        {
            newVehicle.CalculateBestPath(spawnPoint.consequent[0].node, target);
            spawnPoint.vehicles.Add(newVehicle);
            vehicles.Add(newVehicle);
        }
        catch(System.Exception)
        {
            spawnPoint.Mark(true);
            target.Mark(true);
            Debug.Log($"Error while calculating route from {spawnPoint.transform.position} to {target.transform.position}. Vehicle not created");
            DestroyImmediate(newVehicle.gameObject);
            return false;
        }
        return true;
    }

    public void Delete(Vehicle vehicle)
    {
        vehicles.Remove(vehicle);
        DestroyImmediate(vehicle.gameObject);
    }
}
