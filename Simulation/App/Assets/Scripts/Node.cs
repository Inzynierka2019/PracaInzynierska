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

    RoadInfo CreateRoadInfoForVehicle(Vehicle vehicle)
    {
        var info = new RoadInfo();
        int searchDeep = 4;

        List<Node> nextHops = new List<Node>();
        nextHops.Add(this);
        nextHops.Add(vehicle.currentIntermidiateTarget);

        for(int i = 2; i < searchDeep; i++)
        {
            var nextHop = vehicle.GetNextTargetNode(nextHops[i - 1].consequent.Select(c => c.node).ToList());
            if (nextHop == null)
                break;
            nextHops.Add(nextHop);
        }

        float distance = 0f;
        for(int i = 1; i < nextHops.Count; i++)
        {
            Vehicle nearestInFront = null;
            if (i == 1)
                nearestInFront = nextHops[i - 1].FindNearestVehicleInFront(vehicle);
            else
                nearestInFront = nextHops[i - 1].FindNearestVehicleInFront(0, nextHops[i]);

            if(nearestInFront == null)
            {
                distance += nextHops[i - 1].consequent.Find(c => c.node == nextHops[i]).path.length;
                if(nextHops[i].isRedLightOn)
                {
                    info.distanceToNearestObstacle = distance - vehicle.distanceOnCurrentRoadSegment;
                    info.nearestObstacleVelocity = 0;
                    break;
                }
            }
            else
            {
                distance += nearestInFront.distanceOnCurrentRoadSegment;
                info.distanceToNearestObstacle = distance - vehicle.distanceOnCurrentRoadSegment;
                info.nearestObstacleVelocity = nearestInFront.velocity;
                break;
            }
        }

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
        var vehilcesToTransfer = new Dictionary<Vehicle, Node>();
        var vehiclesToRemove = new List<Vehicle>();

        foreach (var vehicle in vehicles)
        {
            var info = CreateRoadInfoForVehicle(vehicle);
            vehicle.UpdatePosition(info);

            if (vehicle.distanceOnCurrentRoadSegment > consequent.Find(c => c.node == vehicle.currentIntermidiateTarget).path.length)
            {
                if (vehicle.currentIntermidiateTarget.consequent.Count == 0)
                    vehiclesToRemove.Add(vehicle);
                else
                {
                    var nextHop = vehicle.GetNextTargetNode(vehicle.currentIntermidiateTarget.consequent.Select(c => c.node).ToList());
                    vehilcesToTransfer.Add(vehicle, vehicle.currentIntermidiateTarget);
                    vehicle.currentIntermidiateTarget = nextHop;
                    vehicle.distanceOnCurrentRoadSegment = 0;
                }
            }
            else
            {
                VertexPath path = consequent.Find(c => c.node == vehicle.currentIntermidiateTarget).path;
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

