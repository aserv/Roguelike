using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {
    public float speed;
    public int damage;
	// Use this for initialization
	void Start () {
        this.GetComponent<Rigidbody2D>().velocity = this.transform.right * speed;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        PlayerController p = collision.gameObject.GetComponent<PlayerController>();
        if (p != null) {
            p.TakeDamage(damage);
        }
        EnemyController e = collision.gameObject.GetComponent<EnemyController>();
        if (e != null) {
            e.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
