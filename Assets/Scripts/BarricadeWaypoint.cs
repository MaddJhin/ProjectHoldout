using UnityEngine;
using System.Collections;

public class BarricadeWaypoint : MonoBehaviour 
{
    public bool occupied;  
    public BoxCollider sCollider;

    private int colliderIndex;
    private GameObject resident;

	// Use this for initialization
	void Start () 
    {
        occupied = false;
        sCollider = GetComponent<BoxCollider>();
	}

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (!occupied && !other.isTrigger)
        {
            Debug.Log("Occupying Waypoint");
            occupied = true;
            resident = other.gameObject;            // Cache the object at the waypoint
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (occupied && other.gameObject == resident && !other.isTrigger)
        {
            Debug.Log("Vacating Waypoint");
            occupied = false;
            resident = null;
        }
    }
}
