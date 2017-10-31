using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBall : MonoBehaviour {

    private Vector3 startPos;

    private void Awake() {
        startPos = transform.position;
    }

	private void Update () {
	    if(Input.GetKeyDown(KeyCode.Space)) {
            transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = startPos;
        }
	}

}
