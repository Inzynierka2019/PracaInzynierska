using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System.Linq;
using System;

public class Node : MonoBehaviour, ISelectable
{
    [SerializeField] Material idleMat;
    [SerializeField] Material selectedMat;

    // ani dictionary ani tuple nie są serializowalne - unity nie zapamiętuje stanu w scenie takich obiektów
    public List<InternodeConnection> consequent = new List<InternodeConnection>();

    [Serializable]
    public struct InternodeConnection
    {
        public Node node;
        public VertexPath path;
        public float weight;

        public InternodeConnection(Node n, VertexPath p, float w)
        {
            node = n;
            path = p;
            weight = w;
        }
    }

    public List<Vehicle> vehicles = new List<Vehicle>();
    public bool isRedLightOn;
    public static int idCounter = 0;
    public int id;

    public void Start()
    {
        id = idCounter++;
    }

    RoadInfo CreateRoadInfoForVehicle(Vehicle vehicle)
    {
        var info = new RoadInfo();
        float distance = 0f;
        var vehiclePathEnumerator = vehicle.path.GetEnumerator();

        while (vehiclePathEnumerator.Current != vehicle.intermediateTarget.Current)
            vehiclePathEnumerator.MoveNext();

        float d = vehicle.distanceOnCurrentRoadSegment;

        var currentNode = this;
        do
        {
            var nearestInFront =
                currentNode.FindNearestVehicleInFront(d, vehiclePathEnumerator.Current);

            d = 0;

            if (nearestInFront == null)
            {
                distance += currentNode.consequent.Find(c => c.node == vehiclePathEnumerator.Current).path.length;
                if (vehiclePathEnumerator.Current.isRedLightOn)
                {
                    info.distanceToNearestObstacle = distance - vehicle.distanceOnCurrentRoadSegment;
                    info.nearestObstacleVelocity = 0;
                    break;
                }

                currentNode = vehiclePathEnumerator.Current;
            }
            else
            {
                distance += nearestInFront.distanceOnCurrentRoadSegment;
                info.distanceToNearestObstacle = distance - vehicle.distanceOnCurrentRoadSegment;
                info.nearestObstacleVelocity = nearestInFront.velocity;
                break;
            }
        } while (vehiclePathEnumerator.MoveNext());

        if (info.distanceToNearestObstacle == float.MaxValue)
        {
            info.distanceToNearestObstacle = distance;
            info.nearestObstacleVelocity = 0;
        }

        return info;
    }

    //Update is called once per frame
    void Update()
    {
        var t = DateTime.Now;
        var t2 = DateTime.Now - t;
        var vehilcesToTransfer = new Dictionary<Vehicle, Node>();
        var vehiclesToRemove = new List<Vehicle>();

        foreach (var vehicle in vehicles)
        {
            var info = CreateRoadInfoForVehicle(vehicle);
            vehicle.UpdatePosition(info);

            if (vehicle.distanceOnCurrentRoadSegment > consequent.Find(c => c.node == vehicle.intermediateTarget.Current).path.length)
            {
                if (vehicle.intermediateTarget.Current.consequent.Count == 0 || vehicle.IsRouteFinished())
                {
                    SimulationManager.SpawnManager.NotifyVehicleRouteFinished(vehicle);
                    vehiclesToRemove.Add(vehicle);
                }
                else
                {
                    var nextHop = vehicle.GetNextTargetNode();
                    vehilcesToTransfer.Add(vehicle, vehicle.intermediateTarget.Current);
                    vehicle.MoveOnPath();
                    vehicle.distanceOnCurrentRoadSegment = 0;
                }
            }
            else
            {
                VertexPath path = consequent.Find(c => c.node == vehicle.intermediateTarget.Current).path;
                vehicle.transform.position = path.GetPointAtDistance(vehicle.distanceOnCurrentRoadSegment, EndOfPathInstruction.Stop);
                vehicle.transform.rotation = path.GetRotationAtDistance(vehicle.distanceOnCurrentRoadSegment, EndOfPathInstruction.Stop);
            }
        }

        foreach (var pair in vehilcesToTransfer)
        {
            vehicles.Remove(pair.Key);
            pair.Value.vehicles.Add(pair.Key);
        }

        foreach (var item in vehiclesToRemove)
        {
            DestroyImmediate(item.gameObject);
            vehicles.Remove(item);
        }
    }

    public void ChangeLightsToGreen()
    {
        isRedLightOn = false;
        GetComponent<Renderer>().material.color = Color.green;
        var newPos = transform.position;
        newPos.z = -2;
        transform.position = newPos;
    }

    public void ChangeLightsToRed()
    {
        isRedLightOn = true;
        GetComponent<Renderer>().material.color = Color.red;
        var newPos = transform.position;
        newPos.z = -2;
        transform.position = newPos;
    }

    public void OnMouseDown()
    {
        if (isRedLightOn)
            ChangeLightsToGreen();
        else
            ChangeLightsToRed();
    }

    //public Vehicle FindNearestVehicleInFront(Vehicle thisVehicle)
    //{
    //    var vehiclesOnRoad = vehicles.FindAll((Vehicle vehicle) =>
    //    {
    //        return vehicle.intermediateTarget.Current == thisVehicle.intermediateTarget.Current && vehicle != thisVehicle;
    //    });

    //    Vehicle result = null;
    //    foreach (var v in vehiclesOnRoad)
    //    {
    //        if (result == null && v.distanceOnCurrentRoadSegment > thisVehicle.distanceOnCurrentRoadSegment)
    //            result = v;
    //        else if (
    //            v.distanceOnCurrentRoadSegment > thisVehicle.distanceOnCurrentRoadSegment &&
    //            v.distanceOnCurrentRoadSegment < result.distanceOnCurrentRoadSegment)
    //        {
    //            result = v;
    //        }
    //    }
    //    return result;
    //}

    public Vehicle FindNearestVehicleInFront(float position, Node target)
    {
        var vehiclesOnRoad = vehicles.FindAll((Vehicle vehicle) =>
        {
            return vehicle.intermediateTarget.Current == target;
        });

        Vehicle result = null;
        foreach (var v in vehiclesOnRoad)
        {
            if (result == null && v.distanceOnCurrentRoadSegment > position)
                result = v;
            else if (
                v.distanceOnCurrentRoadSegment > position &&
                v.distanceOnCurrentRoadSegment < result.distanceOnCurrentRoadSegment)
            {
                result = v;
            }
        }
        return result;
    }

    public void Mark(bool selected)
    {
        if (selected)
            GetComponent<Renderer>().material = selectedMat;
        else
            GetComponent<Renderer>().material = idleMat;
    }

    // used temporarily on junctions
    public void AddConsequent(Node successor)
    {
        if (successor != null)
        {
            if(consequent.Any(c => c.node == successor))
                consequent.RemoveAll(c => c.node == successor);
            else
                consequent.Add(new InternodeConnection(successor, SimulationManager.RoadManager.CreateVertexPath(new Vector3[] { transform.position, (transform.position + successor.transform.position) / 2, successor.transform.position }), 1.0f));
        }
    }
}

