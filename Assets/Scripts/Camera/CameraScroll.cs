using UnityEngine;
using System.Collections;

public class CameraScroll : MonoBehaviour {

	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;

	void Update() {
		float translation = Input.GetAxis("Vertical") * speed;
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		translation *= Time.deltaTime;
		rotation *= Time.deltaTime;
		transform.Translate(Vector3.forward * translation);
		transform.Rotate(0, rotation, 0, Space.World);
	}
}
