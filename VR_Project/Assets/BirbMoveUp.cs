using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirbMoveUp : MonoBehaviour {

	CharacterController controller;

	[SerializeField] private float BirdSpeedMin = 1;
	[SerializeField] private float BirdSpeedMax = 3;
	float speed = 0;

	//[SerializeField] private Transform leftHand, rightHand;
	//[SerializeField] private float grabDistance = 2f;


	// Use this for initialization
	void Start () {
		speed = Random.Range ((float)BirdSpeedMin, (float)BirdSpeedMax);
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		controller.Move (Vector3.up * speed * Time.fixedDeltaTime);
		if (transform.position.y > 50) {
			Destroy (gameObject);
		}
	}
}
