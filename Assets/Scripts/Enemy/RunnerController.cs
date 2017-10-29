using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerController : EnemyController {
	public float wiggleFrequency, wiggleAmplitude;
	int timer = 0;
	// Update is called once per frame
	void FixedUpdate() {
		timer++;
		if (player != null && CloseToPlayer(findDistance)) {
			float theta = pickAngle();
			MoveRadian(theta);
			GetAnimator().SetFloat("X", Mathf.Cos(theta));
			GetAnimator().SetFloat("Y", Mathf.Sin(theta));
		}
	}

	private float pickAngle() {
		Vector2 dir = player.GetComponent<Rigidbody2D>().position - GetComponent<Rigidbody2D>().position;
		dir.Normalize();
		float wiggle = wiggleAmplitude * Mathf.Sin(wiggleFrequency * timer);
		return Mathf.Atan2(dir.y, dir.x) + wiggle;
	}
}
