using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour {

	public float speed;
	public int damageOnHit;
	public int xp;
	protected PlayerController player;
	public int health;

	private Rigidbody2D rb;
	private Animator animator;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
		animator = this.GetComponent<Animator>();
	}

	// act as callbacks in the 'ai' of the more specific enemies

	public Animator GetAnimator() {
		return animator;
	}

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

	public void HurtPlayer(int dmg) {
		player.TakeDamage(dmg);
	}

	public void TakeDamage(int dmg) {
		health -= dmg;
		if (health <= 0) {
			Die();
		}
	}

	public void Die() {
		player.AddExp(xp);
        GameObject.Find("DropManager").GetComponent<DropManager>().Drop(transform.position);
		Destroy(gameObject);
	}
}
