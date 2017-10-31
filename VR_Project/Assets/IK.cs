using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK : MonoBehaviour {
	private Animator anim;
	[SerializeField] private Transform leftController;
	[SerializeField] private Transform rightController;

	private void Start(){
		anim = GetComponent<Animator> ();
	}

	private void OnAnimatorIK(int layerIndex){
		anim.SetIKPositionWeight (AvatarIKGoal.LeftHand, 1);
		anim.SetIKPositionWeight (AvatarIKGoal.RightHand, 1);
		anim.SetIKPosition (AvatarIKGoal.LeftHand, leftController.position);
		anim.SetIKPosition (AvatarIKGoal.RightHand, rightController.position);
	}

}
