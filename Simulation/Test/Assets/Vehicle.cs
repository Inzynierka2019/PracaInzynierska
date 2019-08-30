using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{

    public int hops = 0;
    public float velocity = 0;
    public float currentRoadSegmentDistance;
    public Node currentTarget;

    private void Awake()
    {
        hops = Random.Range(50, 100);
        velocity = Random.Range(0.5f, 1f);
    }

    void Start()
    {
        
    }

    public Node IntersectionDecision(List<Node> intersectionRoutes)
    {
        if (hops-- < 0 || intersectionRoutes.Count == 0)
            return null;
        else
        {
            var newDestination = intersectionRoutes[Random.Range(0, intersectionRoutes.Count)];
            currentTarget = newDestination;
            currentRoadSegmentDistance = 0;
            return newDestination;
        }
    }

    public void UpdatePositionOnRoadSegment()
    {
        currentRoadSegmentDistance += velocity * Time.deltaTime;
    }
}
