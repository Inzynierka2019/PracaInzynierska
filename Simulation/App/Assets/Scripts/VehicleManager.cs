using System.Collections;
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

    public Vehicle Create(Node spawnPoint, Node target, string roadTypeName)
    {
        if (spawnPoint == null || target == null)
        {
            Debug.Log("Could not create vehicle, node is missing.");
            return null;
        }

        Vehicle newVehicle = Instantiate(prefab, spawnPoint.transform.position, Quaternion.identity, transform).GetComponent<Vehicle>();
        newVehicle.roadTypeName = roadTypeName;
        
        newVehicle.CalculateBestPath(spawnPoint.consequent[0].node, target);
        spawnPoint.vehicles.Add(newVehicle);

        vehicles.Add(newVehicle);

        return newVehicle;
    }

    public void Delete(Vehicle vehicle)
    {
        vehicles.Remove(vehicle);
        DestroyImmediate(vehicle.gameObject);
    }
}
