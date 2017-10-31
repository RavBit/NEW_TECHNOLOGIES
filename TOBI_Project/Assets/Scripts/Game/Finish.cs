using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour {

    private int points = 0;

    private List<GameObject> objects = new List<GameObject>();

    public Text txt;

    private void Update() {
        txt.text = "Points: " + points;
    }

    public void AddPoint(GameObject obj) {
        points++;
        obj.gameObject.SetActive(false);
        objects.Add(obj);
    }	

    void OnTriggerEnter(Collider col) {
        points = 0;
        foreach(GameObject obj in objects) {
            obj.gameObject.SetActive(true);
        }
    }

}
