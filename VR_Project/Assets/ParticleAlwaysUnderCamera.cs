using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAlwaysUnderCamera : MonoBehaviour {

	[SerializeField] private Transform cameraTransform;

	void Update () {
		Vector3 pos = transform.position; pos.x = cameraTransform.position.x; pos.z = cameraTransform.position.z;
		transform.position = pos;
	}
}
