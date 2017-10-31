using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annoyingparticles : MonoBehaviour {

    [SerializeField] private float dist = 0.1f;

    private void Update() {
        Vector2 pos = Tobii.Gaming.TobiiAPI.GetGazePoint().Viewport;
        pos += new Vector2(dist, dist);
        transform.position = ToWorld(pos, 10f);
    }


    private Vector3 ToWorld(Vector2 point, float distance)
    {
        Vector3 ray = Camera.main.transform.InverseTransformDirection(TobiiEasyInput.MouseToRay(point, Camera.main).direction);
        float mp = distance / ray.z;
        ray *= mp;
        return ray;
    }

}
