using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {

	public static float rotationSpeed = 3f;

	void FixedUpdate () {
		transform.Rotate (0.0f, 0.0f, rotationSpeed*Time.fixedDeltaTime);
	}
}
