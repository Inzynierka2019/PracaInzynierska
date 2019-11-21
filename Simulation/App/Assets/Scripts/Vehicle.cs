using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public int id;
    public float vehicleLength;
    public List<Node> path;
    public IEnumerator<Node> intermediateTarget;
    public Node start;
    public Node target;
    public string roadTypeName;
    public float tr = 0;

    public Material selectedMaterial;
    public Material slowDriverMaterial;
    public Material normalDriverMaterial;
    public Material aggresiveDriverMaterial;
    public Material currentMaterial;

    public DateTime createTime;

    public Driver driver;

    public float lastDecisionTime;

    public float sumVelocity;
    public float framesCount;

    public bool IsSelected { get; private set; } = false;


    [HideInInspector]
    public static int idsCounter = 0;

    public Decisions? lastDecision = null;

    public enum Decisions
    {
        Accelerate,
        Decelerate,
        KeepVelocity
    }

    public void Awake()
    {
        id = idsCounter++;
        maxVelocity = 60 /*Random.Range(50f, 90f)*/ * 0.27f; //km/h to m/s
        velocity = 0;
        acceleration = 3.5f;/*Random.Range(3f, 4f);*/
        deceleration = 3.5f;/*Random.Range(6f, 7f);*/
        distanceOnCurrentRoadSegment = 0;
        vehicleLength = 5 /*GetComponent<MeshFilter>().mesh.bounds.size.z*/;
        safeDistance = vehicleLength;
        createTime = DateTime.Now;
    }

    public void UpdatePosition(RoadInfo info)
    {
        var myDistanceToFullStop = 0.5f * (Mathf.Pow(velocity, 2) / deceleration);
        var obstacleDistanceToFullStop = 0.5f * (Mathf.Pow(info.nearestObstacleVelocity, 2) / deceleration);

        safeDistance = myDistanceToFullStop - obstacleDistanceToFullStop + 1.5F * vehicleLength;
        myDistanceToFullStop += 1.5f * vehicleLength;

        //Debug.Log($"Car #{id} action: accelerate");
        //Przyspieszamy
        var tmpVelocity = velocity + acceleration * Time.deltaTime;
        if (tmpVelocity > maxVelocity) tmpVelocity = maxVelocity;
        var calculatedMoveDistance = tmpVelocity * Time.deltaTime;
        if (info.distanceToNearestObstacle - calculatedMoveDistance > myDistanceToFullStop && (Time.time - lastDecisionTime > driver.ReactionTime || lastDecision == Decisions.Accelerate || lastDecision == null))
        {
            velocity = tmpVelocity;
            distanceOnCurrentRoadSegment += calculatedMoveDistance;
            if(lastDecision != Decisions.Accelerate)
            {
                lastDecision = Decisions.Accelerate;
                lastDecisionTime = Time.time;
            }
        }
        else
        {
            //Zwalniamy
            tmpVelocity = velocity - deceleration * Time.deltaTime;
            if (tmpVelocity < 0) tmpVelocity = 0;
            calculatedMoveDistance = tmpVelocity * Time.deltaTime;

            if (info.distanceToNearestObstacle - calculatedMoveDistance > vehicleLength)
            {

                if((Time.time - lastDecisionTime > driver.ReactionTime || lastDecision == Decisions.Decelerate))
                {
                    //Debug.Log($"Car #{id} action: breaking");
                    velocity = tmpVelocity;
                    distanceOnCurrentRoadSegment += calculatedMoveDistance;
                    if (lastDecision != Decisions.Decelerate)
                    {
                        lastDecision = Decisions.Decelerate;
                        lastDecisionTime = Time.time;
                    }
                }
                else
                {
                    //Debug.Log($"Car #{id} action: velocity not changed");
                    //Nie zmieniamy
                    calculatedMoveDistance = velocity * Time.deltaTime;
                    distanceOnCurrentRoadSegment += calculatedMoveDistance;
                }
            }
            else
            {
                velocity = 0;
            }
        }

        sumVelocity += velocity;
        framesCount++;
    }

    public Node GetNextTargetNode()
    {
        if(path == null || path.Count == 0)
        {
            Debug.Log("Couldn't get next node on path. Path is empty or non existing");
            return null;
        }

        return path[0];
    }

    public bool IsRouteFinished()
    {
        return intermediateTarget.Current == path.Last();
    }

    public void MoveOnPath()
    {
        intermediateTarget.MoveNext();
    }

    public void CalculateBestPath(Node start, Node target)
    {
        this.start = start;
        this.target = target;

        //item1 = waga krawedzi, item2 = dodatkowa heurystyka (tj. dystans w lini prostej do celu)
        Dictionary<Node, System.Tuple<float, float>> open = new Dictionary<Node, System.Tuple<float, float>>();
        HashSet<Node> closed = new HashSet<Node>();
        Dictionary<Node, Node> path = new Dictionary<Node, Node>();

        open.Add(start, new System.Tuple<float, float>(0, Vector3.Distance(start.transform.position, target.transform.position)));
        open = open.OrderBy(x => x.Value.Item1 + x.Value.Item2).ToDictionary(x => x.Key, x => x.Value);

        while (true)
        {
            var current = open.OrderBy(x => x.Value.Item1 + x.Value.Item2).First();
            open.Remove(current.Key);
            closed.Add(current.Key);

            if (current.Key == target)
                break;

            foreach (var interconnection in current.Key.consequent)
            {
                if (closed.Contains(interconnection.node))
                    continue;

                var newPath =
                    current.Value.Item1 +
                    (1f / interconnection.weight) +
                    Vector3.Distance(current.Key.transform.position, interconnection.node.transform.position);

                if (!open.ContainsKey(interconnection.node) || open[interconnection.node].Item1 + open[interconnection.node].Item2 > newPath)
                {
                    var t = new System.Tuple<float, float>(
                                current.Value.Item1 + interconnection.weight,
                                newPath);

                    path[interconnection.node] = current.Key; // setting "current" as parent of its neighbour node
                    open[interconnection.node] = t;
                }
            }
        }

        this.path.Clear();
        this.path.Add(target);
        var r = path[target];
        while (path[r] != start)
        {
            this.path.Add(r);
            r = path[r];
        }
        this.path.Add(r);

        this.path.Add(start);
        this.path.Reverse();

        intermediateTarget = this.path.GetEnumerator();
        intermediateTarget.MoveNext();
    }

    public void OnMouseUp()
    {
        IsSelected = !IsSelected;
        path.ForEach(n => n.Mark(IsSelected));
        if (IsSelected)
            GetComponent<Renderer>().material.color = selectedMaterial.color;
        else
            GetComponent<Renderer>().material.color = currentMaterial.color;
    }

    public void AssignDriver(Driver driver)
    {
        this.driver = driver;
        switch(driver.Personality)
        {
            case Personality.Slow:
                currentMaterial = slowDriverMaterial;
                break;
            case Personality.Normal:
                currentMaterial = normalDriverMaterial;
                break;
            case Personality.Aggresive:
                currentMaterial = aggresiveDriverMaterial;
                break;
        }

        GetComponent<Renderer>().material.color = currentMaterial.color;
    }
}
