using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {

	[SerializeField] private float HandsFromCamAngle = 30;
	[SerializeField] private float HandsFromDownAngle = 70f;

	[SerializeField] private Transform leftHand, rightHand, _camera, particles;

	[SerializeField] private float moveSpeed = 5, particlemaxangle = -150;

	private CharacterController controller;
	float t = 0;

	void Start(){
		controller = GetComponent<CharacterController> ();
	}

	void FixedUpdate () {
		if (Vector3.Angle (leftHand.position - _camera.position, _camera.transform.forward) <= HandsFromCamAngle &&
		    Vector3.Angle (rightHand.position - _camera.position, _camera.transform.forward) <= HandsFromCamAngle &&
		    Vector3.Angle (leftHand.position - _camera.position, Vector3.down) <= HandsFromDownAngle &&
		    Vector3.Angle (rightHand.position - _camera.position, Vector3.down) <= HandsFromDownAngle) {
			t += Time.fixedDeltaTime;
			t = Mathf.Clamp (t, 0, 1);
			controller.Move (Vector3.ProjectOnPlane (_camera.forward * moveSpeed * Time.fixedDeltaTime, Vector3.up));
			particles.rotation = Quaternion.Lerp (Quaternion.Euler (-90, 0, 0), Quaternion.Euler (particlemaxangle, 0, 0), t);
		} else {
			t -= Time.fixedDeltaTime;
			t = Mathf.Clamp (t, 0, 1);
		}
	}
}
