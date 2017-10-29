using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour {
	public float speed;
	public int damageOnHit;
	public int xp;
	public int health;
    public float findDistance;

    private Rigidbody2D rb;
	private Animator animator;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		animator = this.GetComponent<Animator>();
	}

	// act as callbacks in the 'ai' of the more specific enemies

	public Animator GetAnimator() {
		return animator;
	}

    public void MoveStop()
    {
        rb.velocity = Vector2.zero;
    }

    public void SlowDown(float a)
    {
        rb.velocity = new Vector2(rb.velocity.x * a, rb.velocity.y * a);
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

    public float pickAngle()
    {
        Vector2 dir = GameController.Instance.player.GetComponent<Rigidbody2D>().position - GetComponent<Rigidbody2D>().position;
        dir.Normalize();
        return Mathf.Atan2(dir.y, dir.x);
    }

    public bool CloseToPlayer(float dist) {
		return Vector2.Distance(gameObject.transform.position, GameController.Instance.player.gameObject.transform.position) < dist;
	}

	public void HurtPlayer(int dmg) {
        GameController.Instance.player.TakeDamage(dmg);
	}

	public void TakeDamage(int dmg) {
		health -= dmg;
		if (health <= 0) {
			Die();
		}
	}

	public void Die() {
        GameController.Instance.player.AddExp(xp);
		GameObject.Find("DropManager").GetComponent<DropManager>().Drop(transform.position);
		Destroy(gameObject);
	}
}
