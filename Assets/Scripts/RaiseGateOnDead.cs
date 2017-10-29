using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseGateOnDead : MonoBehaviour {

	public GameObject raiseOnKill;
   
    void Update() {
		if (raiseOnKill == null && GameObject.FindWithTag("Enemy") == null) {
			gameObject.GetComponent<Animator>().SetBool("shouldMoveUp", true);
		}
	}
}
