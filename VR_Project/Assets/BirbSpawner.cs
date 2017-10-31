using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirbSpawner : MonoBehaviour {

	[SerializeField] private float TimeBetweenSpawnsMin = 1;
	[SerializeField] private float TimeBetweenSpawnsMax = 3;
	[SerializeField] private float DistanceToBirdsMin = 2;
	[SerializeField] private float DistanceToBirdsMax = 3;
	[SerializeField] private float BirbAmount = 5;
	[SerializeField] private float DistanceBetweenBirds = 1;
	[SerializeField] private GameObject birbPrefab;

	[SerializeField] private float MapRange = 2;

	private bool isRunning = true;

	private void Start(){
		StartCoroutine (SpawnCoroutine ());
	}

	private void FixedUpdate(){
		//GetComponent<CharacterController> ().Move (-Vector3.up * Time.fixedDeltaTime);
	}

	private void OnApplicationQuit(){
		isRunning = false;
	}

	private void Spawn(){
		List<Vector3> pos = new List<Vector3> ();
		Vector3 playerpos = transform.position;
		for (int i = 0; i < BirbAmount; i++) {
			int tries = 0;
			bool canSpawn = true;
			Vector3 spawnpos = Vector3.zero;
			getpos:{
				tries++;
				canSpawn = true;
				spawnpos = new Vector3 (Random.Range ((float)MapRange/2, (float)-(MapRange/2)) + playerpos.x, playerpos.y - Random.Range((float)DistanceToBirdsMin, (float)DistanceToBirdsMax), Random.Range ((float)2, (float)-2) + playerpos.z);
				for (int o = 0; o < pos.Count; o++) {
						if (Vector3.Distance (spawnpos, pos[o]) < DistanceBetweenBirds) {
						canSpawn = false;
					}
				}
				if(tries <= 10 && !canSpawn){
					goto getpos;
				}
			}
			if (canSpawn) {
				GameObject birb = GameObject.Instantiate (birbPrefab);
				birb.SetActive (true);
				birb.transform.position = spawnpos;
				birb.name = "Birb";
				pos.Add (spawnpos);
			}
		}
	}

	private IEnumerator SpawnCoroutine(){
		do {
			yield return new WaitForSeconds(Random.Range((float)TimeBetweenSpawnsMin, (float)TimeBetweenSpawnsMax));
			print("Spawn");
			Spawn();
		} while(isRunning);
	}

}
