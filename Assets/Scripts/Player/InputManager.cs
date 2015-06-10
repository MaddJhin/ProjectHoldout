using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

	public float surfaceOffset = 1.5f;
	public PlayerCharacterControl setTargetOn;

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
			setTargetOn.SetTarget(hit.point);
		}

		// Place target reticule 
		//transform.position = hit.point + hit.normal*surfaceOffset;
	}
}
