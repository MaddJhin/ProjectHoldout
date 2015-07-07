using UnityEngine;
using System.Collections;

public class CameraScroll : MonoBehaviour {

	public float speed = 10.0F;

	void Update() {
		//Get the Input for the scrolling movement
		float vertical = Input.GetAxis("Vertical") * speed;
		float side = Input.GetAxis("Horizontal") * speed;

		// Adjust for framerate differences
		vertical *= Time.deltaTime;
		side *= Time.deltaTime;

		// Move camera based on world space to ignore camera rotation
		transform.Translate(Vector3.forward * vertical, Space.World);
		transform.Translate(Vector3.right * side, Space.World);
	}
}
