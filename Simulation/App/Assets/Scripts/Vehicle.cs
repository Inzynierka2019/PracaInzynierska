using PathCreation;
using System.Collections.Generic;
using UnityEngine;

public class RoadInfo
{
    public float distanceToNearestObstacle = float.MaxValue;
    public float nearestObstacleVelocity = 0;
}

public class Vehicle : MonoBehaviour
{
    public float maxVelocity;
    public float velocity;
    public float acceleration;
    public float deceleration;
    public float safeDistance;
    public float distanceOnCurrentRoadSegment;
    public Node currentIntermidiateTarget;
    public int id;
    public float vehicleLength;

    [HideInInspector]
    public static int idsCounter = 0;

    public void Awake()
    {
        id = idsCounter++;
        maxVelocity = Random.Range(50f, 90f) * 0.27f; //km/h to m/s
        velocity = 0;
        acceleration = Random.Range(3f, 4f);
        deceleration = Random.Range(6f, 7f);
        distanceOnCurrentRoadSegment = 0;
        vehicleLength = 5 /*GetComponent<MeshFilter>().mesh.bounds.size.z*/;
        safeDistance = vehicleLength;
    }

    public void UpdatePosition(RoadInfo info)
    {
        var myDistanceToFullStop = 0.5f * (Mathf.Pow(velocity, 2) / deceleration);
        var obstacleDistanceToFullStop = 0.5f * (Mathf.Pow(info.nearestObstacleVelocity, 2) / deceleration);

        safeDistance = myDistanceToFullStop - obstacleDistanceToFullStop + 1.5F * vehicleLength;

        //Debug.Log($"Car #{id} action: accelerate");
        //Przyspieszamy
        var tmpVelocity = velocity + acceleration * Time.deltaTime;
        if (tmpVelocity > maxVelocity) tmpVelocity = maxVelocity;
        var calculatedMoveDistance = tmpVelocity * Time.deltaTime;
        if (info.distanceToNearestObstacle - calculatedMoveDistance > safeDistance)
        {
            velocity = tmpVelocity;
            distanceOnCurrentRoadSegment += calculatedMoveDistance;
        }
        else
        {
            //Debug.Log($"Car #{id} action: velocity not changed");
            //Nie zmieniamy
            calculatedMoveDistance = velocity * Time.deltaTime;
            if (info.distanceToNearestObstacle - calculatedMoveDistance > safeDistance)
                distanceOnCurrentRoadSegment += calculatedMoveDistance;
            else
            {
                //Zwalniamy
                tmpVelocity = velocity - deceleration * Time.deltaTime;
                if (tmpVelocity < 0) tmpVelocity = 0;
                calculatedMoveDistance = tmpVelocity * Time.deltaTime;

                if (info.distanceToNearestObstacle - calculatedMoveDistance > vehicleLength)
                {
                    //Debug.Log($"Car #{id} action: breaking");
                    velocity = tmpVelocity;
                    distanceOnCurrentRoadSegment += calculatedMoveDistance;
                }
                else
                    velocity = 0;
            }
        }
    }

    public Node GetNextTargetNode(List<Node> nodes)
    {
        if (nodes.Count != 0)
            return nodes[Random.Range(0, nodes.Count)];
        else
            return null;
    }
}

