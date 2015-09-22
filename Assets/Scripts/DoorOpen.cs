using UnityEngine;
using System.Collections;

public class DoorOpen : MonoBehaviour 
{		
	public float speed;

    [Tooltip ("Radius in which a unit is checked for eligibility to open a door")]
    public float triggerRadius = 3f;

    [Tooltip ("Distance a unit must be within from the door in order for it to open")]
    public float openTreshold = 1f;

    [Tooltip("Distance a unit must be within from the door in order for it to close")]
    public float closeThreshold = 2f;

    private bool open = false;
    private Vector3 destination;
    private int playerMask;
	private Vector3 startMarker;
	private Vector3 originalPosition;

    void Awake()
    {
        playerMask = LayerMask.GetMask("Player");
    }

	void Start()
    {
		originalPosition = transform.localPosition;
	}

    void CheckTrigger()
    {
        Collider[] possibleTriggers = Physics.OverlapSphere(transform.position, triggerRadius, playerMask);

        foreach (Collider currTrigger in possibleTriggers)
        {
            // Valid target to open the door
            if (currTrigger.tag == "Player")
            {
                float distanceToDoor = Vector3.Distance(currTrigger.gameObject.transform.position, transform.position);
                
                // If target is halfway through the trigger radius, open the door
                if (distanceToDoor <= openTreshold && !open)
                {
                    OpenDoor();
                }

                else if (distanceToDoor >= closeThreshold && open)
                {
                    CloseDoor();
                }
            }
        }
    }

    void OpenDoor()
    {
        open = true;
        startMarker = transform.position;
        destination = new Vector3(transform.position.x, startMarker.y + 5, transform.position.z);
        transform.localPosition = Vector3.Lerp(startMarker, destination, speed);
    }

    void CloseDoor()
    {
        open = false;
        startMarker = transform.position;
        destination = new Vector3(transform.position.x, startMarker.y + 5, transform.position.z);
        transform.localPosition = Vector3.Lerp(startMarker, destination, speed);
    }
}
