using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System.Linq;

public class Node : MonoBehaviour
{
    public Dictionary<Node, VertexPath> consequent = new Dictionary<Node, VertexPath>();

    public List<Vehicle> vehicles = new List<Vehicle>();

    //Update is called once per frame
    void Update()
    {
        var vehiclesToTransfer = new Dictionary<Vehicle, Node>();
        var vehiclesToRemove = new List<Vehicle>();
        foreach (var vehicle in vehicles)
        {
            // Tutaj można poinformować pojazd o sytuacji przed nim.
            vehicle.UpdatePositionOnRoadSegment();
            if (vehicle.currentRoadSegmentDistance > 1)
            {
                //Tutaj można powiedzieć pojazdowi, że poruszanie się to tego węzła jest w tej chwili nie możliwe
                //np. nie można teraz zmienić pasa ruchu lub stoi na skrzyżowaniu i jest czerwone światło
                var vehicleArrivedAtThisNode = vehicle.currentTarget;
                var decision = vehicle.IntersectionDecision(vehicle.currentTarget.consequent.Keys.ToList());
                if (decision == null)
                    vehiclesToRemove.Add(vehicle);
                else
                    vehiclesToTransfer.Add(vehicle, vehicleArrivedAtThisNode);
            }
            else
            {
                var path = consequent[vehicle.currentTarget];
                var newPosition = path.GetPoint(vehicle.currentRoadSegmentDistance, EndOfPathInstruction.Stop);
                vehicle.transform.position = newPosition;
            }
        }

        foreach (var v in vehiclesToRemove)
        {
            vehicles.Remove(v);
            Destroy(v.gameObject);
        }

        foreach (var pair in vehiclesToTransfer)
        {
            vehicles.Remove(pair.Key);
            pair.Value.vehicles.Add(pair.Key);
        }
    }
}
