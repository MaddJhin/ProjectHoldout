using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public float surfaceOffset = 1.5f;
	public PlayerCharacterControl setTargetOn;

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

		// Grab the player script component of the player selected
		if (hit.collider.tag == "Player")
		{
			setTargetOn = hit.transform.gameObject.GetComponent<PlayerCharacterControl>();
		}

		// If player selects barricade, call SetTarget on the selected player character, checking for null.
		else if (hit.collider.tag == "Barricade" && setTargetOn != null)
		{
			setTargetOn.SetDestination(hit.transform);
		}

		// Place target reticule 
		//transform.position = hit.point + hit.normal*surfaceOffset;
	}

	public void SetTarget (PlayerCharacterControl player){
		setTargetOn = player;
	}
}
