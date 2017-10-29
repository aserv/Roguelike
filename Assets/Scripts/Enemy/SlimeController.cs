using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : EnemyController {
	// Update is called once per frame
	void FixedUpdate() {
		if (GameController.Instance.player != null && CloseToPlayer(findDistance)) {
			MoveRadian(pickAngle());
		}
	}

	private float pickAngle() {
		Vector2 dir = GameController.Instance.player.GetComponent<Rigidbody2D>().position - GetComponent<Rigidbody2D>().position;
		dir.Normalize();
		return Mathf.Atan2(dir.y, dir.x);
	}
}
