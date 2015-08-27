using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {

	public Camera thirdPersonCam; 				// Holds the main camera used in third person view
	public Camera tacticalCam;					// Hold the secondary ortographic camera for tactical view
	public PlayerMovement setTargetOn;

	private bool thirdPerson;
	private Camera activeCam;
    private BarricadeWaypoint waypoint_cache;
    private BarricadeWaypoint[] waypointList;
    private List<Light> waypointMarkerList;

    #region Singleton
    private static InputManager _instance;

    public static InputManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<InputManager>();

                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {

            Debug.Log("Creating singleton");
            // If this instance is the first in the scene, it becomes the singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }

        else
        {
            // If another Singleton already exists, destroy this object
            if (this != _instance)
            {
                Debug.Log("Destroying invalid singleton");
                Destroy(this.gameObject);
            }
        }

        waypointMarkerList = new List<Light>();


    }
    #endregion

    void Start()
    {
		thirdPersonCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		tacticalCam = GameObject.FindGameObjectWithTag("TacticalCamera").GetComponent<Camera>();
        waypointList = FindObjectsOfType<BarricadeWaypoint>();

        foreach (var waypoint in waypointList)
        {
            waypointMarkerList.Add(waypoint.GetComponent<Light>());
            Debug.Log(waypoint);
        }

		thirdPersonCam.enabled = true;
		tacticalCam.enabled = false;
		thirdPerson = true;
		activeCam = thirdPersonCam;
	}

    void Update () {
		// Run when user clicks
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}

		// Raycast to mouse
		Ray ray = activeCam.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		// Return if nothing was hit
        if (!Physics.Raycast(ray, out hit))
        {
            setTargetOn = null;
        }

        // If player selects barricade, call SetTarget on the selected player character, checking for null.
        else if (thirdPerson)
        {
            waypoint_cache = hit.collider.gameObject.GetComponent<BarricadeWaypoint>();

            if (hit.collider.tag == "Waypoint" && setTargetOn != null)
            {

                StartCoroutine("DisableWaypointLights");
                Debug.Log("Waypoint Found");
                setTargetOn.SetDestination(hit.transform);
                waypoint_cache.sCollider.isTrigger = true;
            }
        }
		else if (!thirdPerson)
		{
			Vector3 location = new Vector3 (hit.point.x, thirdPersonCam.transform.parent.position.y, hit.point.z);
			thirdPersonCam.transform.parent.position = location;

			ChangePerspective();
		}
	}

	public void SetTarget (PlayerMovement player)
    {
        StartCoroutine("EnableWaypointLights");
        Debug.Log("Target set to: " + player);
		setTargetOn = player;
	}

    IEnumerator EnableWaypointLights()
    {
        foreach (var waypoint in waypointMarkerList)
        {
            waypoint.enabled = true;
        }

        yield return null;
    }

    IEnumerator DisableWaypointLights()
    {
        foreach (var waypoint in waypointMarkerList)
        {
            waypoint.enabled = false;
        }

        yield return null;
    }

	public void ChangePerspective (){
		thirdPersonCam.enabled = !thirdPersonCam.enabled;
		tacticalCam.enabled = !tacticalCam.enabled;
		thirdPerson = !thirdPerson;
		if (thirdPerson)
		{
			activeCam = thirdPersonCam;
		}
		else
		{
			activeCam = tacticalCam;
		}
	}
}
