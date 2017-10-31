using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    public Finish fin;

    private void OnTriggerEnter(Collider col) {
        fin.AddPoint(gameObject);
    }

}
