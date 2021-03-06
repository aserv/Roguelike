using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyebatController : EnemyController {
	public float wiggleFrequency, wiggleAmplitude;
	int timer = 0;
	// Update is called once per frame
	void FixedUpdate() {
		timer++;
		if (GameController.Instance.player != null && CloseToPlayer(findDistance)) {
			float theta = pickWiggleAngle();
			MoveRadian(theta);
			GetAnimator().SetFloat("X", Mathf.Cos(theta));
			GetAnimator().SetFloat("Y", Mathf.Sin(theta));
		}
	}

	private float pickWiggleAngle() {
		Vector2 dir = GameController.Instance.player.GetComponent<Rigidbody2D>().position - GetComponent<Rigidbody2D>().position;
		dir.Normalize();
		float wiggle = wiggleAmplitude * Mathf.Sin(wiggleFrequency * timer);
		return Mathf.Atan2(dir.y, dir.x) + wiggle;
	}
}
