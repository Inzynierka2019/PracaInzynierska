using PathCreation;
using System.Collections.Generic;
using UnityEngine;

public class RoadInfo
{
    public bool isRoadClear = false;
    public float distanceToNearestObstacle = 0;
    public float nearestObstacleVelocity = 0;
}

public class Vehicle : MonoBehaviour
{
    public float maxVelocity;
    public float velocity;
    public float acceleration;
    public float deceleration;
    public float safeDistance;
    public float criticalDistance;
    public float distanceOnCurrentRoadSegment;
    public VertexPath currentPath;
    public Node currentIntermidiateTarget;
    public bool isWaitingAtNode = false;
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
        deceleration = Random.Range(4f, 6f);
        criticalDistance = 1f;
        distanceOnCurrentRoadSegment = 0;
        vehicleLength = GetComponent<MeshFilter>().mesh.bounds.size.y;
        safeDistance = vehicleLength;
    }

    public void UpdatePosition(RoadInfo info)
    {
        if (info.isRoadClear)
        {
            //Debug.Log($"Car #{id} action: accelerate");
            velocity += acceleration * Time.deltaTime;
            if (velocity > maxVelocity) velocity = maxVelocity;

            var calculatedMoveDistance = velocity * Time.deltaTime;
            distanceOnCurrentRoadSegment += calculatedMoveDistance;
        }
        else
        {
            var myDistanceToFullStop = 0.5f * (Mathf.Pow(velocity, 2) / deceleration);
            var obstacleDistanceToFullStop = 0.5f * (Mathf.Pow(info.nearestObstacleVelocity, 2) / deceleration);

            safeDistance = myDistanceToFullStop - obstacleDistanceToFullStop + vehicleLength * 0.7f;

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
                    Debug.Log($"Car #{id} action: breaking");
                    //Zwalniamy
                    tmpVelocity = velocity - deceleration * Time.deltaTime;
                    if (tmpVelocity < 0) tmpVelocity = 0;
                    calculatedMoveDistance = tmpVelocity * Time.deltaTime;
                    if (info.distanceToNearestObstacle - calculatedMoveDistance > safeDistance)
                        distanceOnCurrentRoadSegment += calculatedMoveDistance;
                    else
                    {
                        //Stoimy
                        //Debug.Log($"Car #{id} action: full stop");
                        velocity = 0;
                    }
                }
            }
        }
    }

    public Node GetNextTargetNode(List<Node> nodes)
    {
        return nodes[Random.Range(0, nodes.Count)];
    }
}

