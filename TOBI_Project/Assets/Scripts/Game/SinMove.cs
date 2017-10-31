using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMove : MonoBehaviour {

    private Vector3 startPos;
    [SerializeField] private float offset = 2;

    private void Awake() {
        startPos = transform.position;
    }

    private void Update() {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time) * offset;
    }	

}
