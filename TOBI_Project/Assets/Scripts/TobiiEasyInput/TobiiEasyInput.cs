using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using tobii = Tobii.Gaming.TobiiAPI;

public static class TobiiEasyInput {

    public static Vector2 MousePostion {
        get {
            if(tobii.GetUserPresence() == Tobii.Gaming.UserPresence.Present) {
                return tobii.GetGazePoint().Viewport;
            }
            else {
                return new Vector2(1f / (float)Screen.width * (float)Input.mousePosition.x, 1f / (float)Screen.height * (float)Input.mousePosition.y);
            }
        }
    }


    public static Ray MouseToRay(Vector2 viewportPos, Camera cam) {
        return cam.ViewportPointToRay(viewportPos);
    }

}
