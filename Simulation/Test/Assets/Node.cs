using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System;
using Random = UnityEngine.Random;

public class Node : MonoBehaviour
{
    public List<Node> neighbours = new List<Node>();
    public Dictionary<Node, VertexPath> paths = new Dictionary<Node, VertexPath>();
    public List<List<Node>> outsideConnections = new List<List<Node>>();
    public List<Vehicle> vehicles = new List<Vehicle>();
    private static int id = 0;

    public void ClearConnectionsAndPaths()
    {
        paths.Clear();
        outsideConnections.Clear();
    }

    public void OnMouseDown()
    {
        Debug.Log($"Creating vehicle on {name}");
        var newVehicle = Instantiate(Resources.Load<GameObject>("VehiclePrefab"));
        var outsideConnection = outsideConnections[Random.Range(0, outsideConnections.Count)];
        var sourceNode = outsideConnection[Random.Range(0, outsideConnection.Count)];

        newVehicle.GetComponent<Vehicle>().currentTarget = sourceNode.neighbours[Random.Range(0, sourceNode.neighbours.Count)];
        newVehicle.transform.position = sourceNode.transform.position;
        sourceNode.GetComponent<Node>().vehicles.Add(newVehicle.GetComponent<Vehicle>());
    }

    private GameObject CreateSmallNodeAtPosition(Vector3 position)
    {
        var smallNode = Instantiate(Resources.Load<GameObject>("SmallNodePrefab"));
        smallNode.name = (id++).ToString();
        smallNode.transform.position = position;
        smallNode.tag = ObjectBuilderEditor.Tags.Untagged.ToString();
        smallNode.transform.parent =
                ObjectBuilderEditor.GetNodesContainer(ObjectBuilderEditor.Containers.SmallNodesContainer).transform;

        return smallNode;
    }

    private VertexPath CreateVertexPath(Vector3[] points)
    {
        BezierPath bezierPath = new BezierPath(points, isClosed: false, PathSpace.xy);
        return new VertexPath(bezierPath);
    }

    public void AddConnection(List<Node> newConnection)
    {
        foreach(var connection in outsideConnections)
        {
            for (var j = 0; j < connection.Count; j++)
            {
                try
                {
                    var end = newConnection[j].transform;
                    var start = connection[j].transform;
                    var path = CreateVertexPath(new Vector3[] { start.position, transform.position, end.position });
                    start.GetComponent<Node>().neighbours.Add(end.GetComponent<Node>());
                    start.GetComponent<Node>().paths.Add(end.GetComponent<Node>(), path);
                }
                catch (Exception e)
                {
                    Debug.Log($"Number of end connections in both nodes '{name}' must match.");
                    Debug.LogError(e.ToString());
                }
            }
        }
        outsideConnections.Add(newConnection);
    }

    public void AddNeighbour(Node neighbourNode)
    {
        neighbours.Add(neighbourNode);

        var vertexPath = CreateVertexPath(
            new Vector3[] {
                transform.position,
                (transform.position + neighbourNode.transform.position) / 2,
                neighbourNode.transform.position });

        var startDistance = 0.2f;
        var step = 0.1f;

        var currentSmallNode = CreateSmallNodeAtPosition(vertexPath.GetPoint(startDistance, EndOfPathInstruction.Stop));
        AddConnection(new List<Node>() { currentSmallNode.GetComponent<Node>() });

        for(var i = startDistance; i <= 1 - startDistance; i += step)
        {
            Vector3[] pathSegment = new Vector3[3];
            pathSegment[0] = vertexPath.GetPoint(i, EndOfPathInstruction.Stop); //start
            pathSegment[1] = vertexPath.GetPoint(i + step/2, EndOfPathInstruction.Stop); //middle
            pathSegment[2] = vertexPath.GetPoint(i + step, EndOfPathInstruction.Stop); //end

            var smallNode = CreateSmallNodeAtPosition(pathSegment[2]);

            var segmentVertexPath = CreateVertexPath(pathSegment);
            currentSmallNode.GetComponent<Node>().paths.Add(smallNode.GetComponent<Node>(), segmentVertexPath);
            currentSmallNode.GetComponent<Node>().neighbours.Add(smallNode.GetComponent<Node>());
            currentSmallNode = smallNode;

            if (i + step >= 1 - startDistance)
            {
                neighbourNode.AddConnection(new List<Node>() { currentSmallNode.GetComponent<Node>() });
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
                var decision = vehicle.IntersectionDecision(vehicle.currentTarget.neighbours);
                if (decision == null)
                    vehiclesToRemove.Add(vehicle);
                else
                    vehiclesToTransfer.Add(vehicle, vehicleArrivedAtThisNode);
            }
            else
            {
                var path = paths[vehicle.currentTarget];
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
