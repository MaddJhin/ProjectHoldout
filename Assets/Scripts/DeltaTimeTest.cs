using UnityEngine;
using System.Collections;

public class DeltaTimeTest : MonoBehaviour {

	float elapsedTime;
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		Debug.Log ("Elapsed Time: " + elapsedTime);
		Debug.Log ("Time Passed: "  + Time.time);
	}
}
