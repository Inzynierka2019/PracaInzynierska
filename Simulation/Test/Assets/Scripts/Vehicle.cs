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
    public float velocity;
    public float acceleration;
    public float deceleration;
    public float safeDistance;
    public float criticalDistance;
    public float distanceOnCurrentRoadSegment;
    public VertexPath currentPath;
    public Node currentIntermidiateTarget;
    public bool isWaitingAtNode = false;

    public void Awake()
    {
        velocity = Random.Range(1f, 7);
        acceleration = 2;
        deceleration = 2;
        safeDistance = 2.5f;
        criticalDistance = 1.5f;
        distanceOnCurrentRoadSegment = 0;
    }

    public void UpdateRoadInfo(RoadInfo info)
    {
        var calculatedMoveDistance = velocity * Time.deltaTime;

        if (info.isRoadClear)
        {
            distanceOnCurrentRoadSegment += calculatedMoveDistance;
        }
        else
        {
            if (info.distanceToNearestObstacle - calculatedMoveDistance > safeDistance)
                distanceOnCurrentRoadSegment += calculatedMoveDistance;
        }
    }

    public Node GetNextTargetNode(List<Node> nodes)
    {
        return nodes[Random.Range(0, nodes.Count)];
    }
}

