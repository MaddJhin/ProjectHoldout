using UnityEngine;
using System.Collections;

public class BarricadeWaypoint : MonoBehaviour 
{
    public bool occupied;  
	public GameObject resident;                    // Reference to the object occupying the barricade

    private int colliderIndex;
	
	// Use this for initialization
	void Awake () 
    {
        occupied = false;
	}

    /* Function: Checks if the collision is a legitimate resident
     * Parameters: Collider of potential resident
     * Returns: None
     *
    void OnTriggerEnter(Collider other)
    {
        // If the waypoint isn't occupied, and the potential resident is a player ...
        if (other.gameObject.tag == "Player" && !other.isTrigger && !occupied)
        {
            Debug.Log("Occupying Waypoint");
            occupied = true;
            resident = other.gameObject;            // Cache the object at the waypoint
        }
    }

    /* Function: Checks if the object leaving the collider is the previous resident
     * Parameters: Collider of previous resident
     * Returns: None
     *
    void OnTriggerExit(Collider other)
    {
        // If the waypoint is already occupied and the object leaving is the previous resident ...
        if (occupied && other.gameObject == resident && !other.isTrigger)
        {
            Debug.Log("Vacating Waypoint");
            occupied = false;
            resident = null;
        }
    }*/
}
