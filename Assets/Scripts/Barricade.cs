using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Barricade : MonoBehaviour 
{
    public List<BarricadeWaypoint> frontWaypoints;
    public List<BarricadeWaypoint> backWaypoints;

	// Use this for initialization
	void Start () 
    {
        BarricadeWaypoint[] temp = GetComponentsInChildren<BarricadeWaypoint>();

        foreach(var curr in temp)
        {
            if (curr.tag == "Front Waypoint")
                frontWaypoints.Add(curr);

            else
                backWaypoints.Add(curr);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AssignWaypoint(string waypointLoc, GameObject unit)
    {
        PlayerMovement barricadeCache;

        if (barricadeCache = unit.GetComponent<PlayerMovement>())
        {
            if (waypointLoc == "front")
            {
                foreach (var waypoint in frontWaypoints)
                {
                    if (waypoint.occupied = false)
                    {
                        if (barricadeCache.targetWaypoint != null)
                        {
                            barricadeCache.targetWaypoint.occupied = false;
                            barricadeCache.targetWaypoint.resident = null;
                        }

                        barricadeCache.targetWaypoint = waypoint;
                        waypoint.occupied = true;
                        waypoint.resident = unit;
                    }
                }
            }

            else
            {
                foreach (var waypoint in frontWaypoints)
                {
                    if (waypoint.occupied = false)
                    {
                        if (barricadeCache.targetWaypoint != null)
                        {
                            barricadeCache.targetWaypoint.occupied = false;
                            barricadeCache.targetWaypoint.resident = null;
                        }

                        barricadeCache.targetWaypoint = waypoint;
                        waypoint.occupied = true;
                        waypoint.resident = unit;
                    }
                }
            }
        }
    }
}
