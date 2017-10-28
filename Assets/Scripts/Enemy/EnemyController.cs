using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	public float speed;
	public float damageOnHit;
	public int xp;
	public PlayerController player;

	private Rigidbody2D rb;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	// act as callbacks in the 'ai' of the more specific enemies

	public void MoveUp() { 
		MoveDegree(90.0f);
	}

	public void MoveDown() {
		MoveDegree(270.0f);
	}

	public void MoveRight() {
		MoveDegree(0.0f);
	}

	public void MoveLeft() {
		MoveDegree(180.0f);
	}

	public void MoveDegree(float angle) {
		MoveRadian(Mathf.Deg2Rad * angle);
	}

	public void MoveRadian(float angle) {
		rb.MoveRotation((angle * Mathf.Rad2Deg - 90) % 360);
		rb.velocity = new Vector2(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed);
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Player")) {
			HurtPlayer(damageOnHit);
		}
	}

	public void HurtPlayer(float dmg) {
		player.TakeDamage(dmg);
	}

	public void Die() {
		player.AddExp(xp);
	}
}
