using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public float surfaceOffset = 1.5f;
	public PlayerMovement setTargetOn;

    private BarricadeWaypoint waypoint_cache;

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
    }
    #endregion

    void Update () {
		// Run when user clicks
		if (!Input.GetMouseButtonDown(0))
		{
			return;
		}

		// Raycast to mouse
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		// Return if nothing was hit
        if (!Physics.Raycast(ray, out hit))
        {
            return;
        }

        // If player selects barricade, call SetTarget on the selected player character, checking for null.
        else
        {
            waypoint_cache = hit.collider.gameObject.GetComponent<BarricadeWaypoint>();

            if (hit.collider.tag == "Waypoint" && setTargetOn != null)
            {
                Debug.Log("Waypoint Found");
                setTargetOn.SetDestination(hit.transform);
                waypoint_cache.sCollider.isTrigger = true;
            }
        }

		// Place target reticule 
		//transform.position = hit.point + hit.normal*surfaceOffset;
	}

	public void SetTarget (PlayerMovement player)
    {
        Debug.Log("Target set to: " + player);
		setTargetOn = player;
	}
}
