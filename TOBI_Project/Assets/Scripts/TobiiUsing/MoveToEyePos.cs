using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tobii = Tobii.Gaming.TobiiAPI;

public class MoveToEyePos : MonoBehaviour {

    public GameObject obj;
    public Camera cam;

	void FixedUpdate () {
        if (tobii.GetUserPresence() == Tobii.Gaming.UserPresence.Present) {
            obj.transform.position = cam.transform.position + cam.ViewportPointToRay(tobii.GetGazePoint().Viewport).direction * 5;
        }
        else {
            obj.transform.position = cam.transform.position + cam.ScreenPointToRay(Input.mousePosition).direction * 5;
        }
	}

}
