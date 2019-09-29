using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System.Linq;

public class Node : MonoBehaviour
{
    public Dictionary<Node, VertexPath> consequent = new Dictionary<Node, VertexPath>();

    public List<Vehicle> vehicles = new List<Vehicle>();
    public bool occupied;
    public object occupiedGuard = new object();

    //Update is called once per frame
    void Update()
    {
        var vehilcesToTransfer = new Dictionary<Vehicle, Node>();
        var vehiclesToRemove = new List<Vehicle>();

        foreach (var vehicle in vehicles)
        {
            if (!vehicle.isWaitingAtNode)
            {
                var nearestInFront = FindNearestVehicleInFront(vehicle);
                var info = new RoadInfo();
                info.isRoadClear = nearestInFront == null ? true : false;
                if (!info.isRoadClear)
                {
                    info.distanceToNearestObstacle = nearestInFront.distanceOnCurrentRoadSegment - vehicle.distanceOnCurrentRoadSegment;
                    info.nearestObstacleVelocity = nearestInFront.velocity;
                }
                else
                {
                    if (vehicle.currentIntermidiateTarget.occupied)
                    {
                        info.isRoadClear = false;
                        info.distanceToNearestObstacle = consequent[vehicle.currentIntermidiateTarget].length - vehicle.distanceOnCurrentRoadSegment;
                        info.nearestObstacleVelocity = 0;
                    }
                }
                vehicle.UpdatePosition(info);
            }

            if (vehicle.distanceOnCurrentRoadSegment > consequent[vehicle.currentIntermidiateTarget].length)
            {
                if (vehicle.currentIntermidiateTarget.consequent.Count == 0)
                    vehiclesToRemove.Add(vehicle);
                else
                {
                    var nextHop = vehicle.GetNextTargetNode(vehicle.currentIntermidiateTarget.consequent.Keys.ToList());
                    var nearestInFront = vehicle.currentIntermidiateTarget.FindNearestVehicleInFront(0, nextHop);
                    vehicle.isWaitingAtNode = true;
                    lock (vehicle.currentIntermidiateTarget.occupiedGuard)
                    {
                        vehicle.currentIntermidiateTarget.occupied = true;
                    }

                    if (nearestInFront == null || nearestInFront.distanceOnCurrentRoadSegment > vehicle.safeDistance)
                    {
                        vehilcesToTransfer.Add(vehicle, vehicle.currentIntermidiateTarget);
                        lock (vehicle.currentIntermidiateTarget.occupiedGuard)
                        {
                            vehicle.currentIntermidiateTarget.occupied = false;
                        }
                        vehicle.currentIntermidiateTarget = nextHop;
                        vehicle.distanceOnCurrentRoadSegment = 0;
                        vehicle.isWaitingAtNode = false;
                    }
                }
            }
            else
            {
                vehicle.transform.position = consequent[vehicle.currentIntermidiateTarget].GetPointAtDistance(vehicle.distanceOnCurrentRoadSegment, EndOfPathInstruction.Stop);
                vehicle.transform.rotation = consequent[vehicle.currentIntermidiateTarget].GetRotationAtDistance(vehicle.distanceOnCurrentRoadSegment, EndOfPathInstruction.Stop);
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

    public Vehicle FindNearestVehicleInFront(Vehicle thisVehicle)
    {
        var vehiclesOnRoad = vehicles.FindAll((Vehicle vehicle) =>
        {
            return vehicle.currentIntermidiateTarget == thisVehicle.currentIntermidiateTarget && vehicle != thisVehicle;
        });

        Vehicle result = null;
        foreach (var v in vehiclesOnRoad)
        {
            if (result == null && v.distanceOnCurrentRoadSegment > thisVehicle.distanceOnCurrentRoadSegment)
                result = v;
            else if (
                v.distanceOnCurrentRoadSegment > thisVehicle.distanceOnCurrentRoadSegment &&
                v.distanceOnCurrentRoadSegment < result.distanceOnCurrentRoadSegment)
            {
                result = v;
            }
        }
        return result;
    }

    public Vehicle FindNearestVehicleInFront(float position, Node target)
    {
        var vehiclesOnRoad = vehicles.FindAll((Vehicle vehicle) =>
        {
            return vehicle.currentIntermidiateTarget == target;
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
}

