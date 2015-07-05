using UnityEngine;
using System.Collections;

public class DoorOpen : MonoBehaviour {
	
	public Vector3 destination;
	public float speed;
	private Vector3 startMarker;
	private Vector3 originalPosition;

	void Start(){
		originalPosition = transform.localPosition;
	}

	void OnTriggerEnter (Collider other){
		if(other.tag == "Player")
		{
			startMarker = transform.position;
			transform.localPosition = Vector3.Lerp(startMarker, destination, speed);
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Player")
		{
			startMarker = transform.position;
			transform.localPosition = Vector3.Lerp(startMarker, originalPosition, speed);
		}
	}
}
