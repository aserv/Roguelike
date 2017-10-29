using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    [Serializable]
    public class PlayerData {
        public float moveSpeed;
        public int health;
        public int maxHealth;
        public int exp;
        public int nextexp;
        public int lvl;
        public BaseItem[] items = new BaseItem[4];
    }
    public PlayerData data = new PlayerController.PlayerData();
    public float punchDistance;
    public int punchStrength;

    private int nextItem = 0;
	private Rigidbody2D rb;
    private Vector2 facing;
	private Animator animator;

	// Use this for initialization
	void Start() {
		rb = this.GetComponent<Rigidbody2D>();
		animator = this.GetComponent<Animator>();
	}

	void Update() {
		if (data.items [0] != null && Input.GetButtonDown("Use1")) {
			UseItem(0);
		}
		if (data.items [1] != null && Input.GetButtonDown("Use2")) {
			UseItem(1);
		}
		if (data.items [2] != null && Input.GetButtonDown("Use3")) {
			UseItem(2);
		}
		if (data.items [3] != null && Input.GetButtonDown("Use4")) {
			UseItem(3);
		}
		if (Input.GetButtonDown("Attack")) {
			Attack();
		}
	}

	// Update is called once per frame
	void FixedUpdate() {
		Vector2 input= Snap(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
		rb.velocity = input * data.moveSpeed;
		if (input != Vector2.zero) {
            facing = input;
            animator.SetFloat("X", facing.x);
			animator.SetFloat("Y", facing.y);
		}
	}

    public Vector2 Facing() {
        return facing;
    }

	public void AddExp(int xp) {
		data.exp += xp;
	}

	public bool PickupItem(BaseItem i) {
		if (nextItem == -1)
			return false;
		data.items [nextItem] = i;
		for (int n = 3; n >= 0; n--) {
			if (data.items [n] == null)
				nextItem = n;
		}
		return true;
	}

	public BaseItem GetItemAt(int i) {
		return data.items [i];
	}

	private void UseItem(int i) {
		if (data.items [i] == null)
			return;
		BaseItem.Result res = data.items [i].Use(this);
		if (res == BaseItem.Result.Consumed) {
			if (nextItem == -1 || nextItem > i)
				nextItem = i;
			data.items [i] = null;
		}
	}

	public void TakeDamage(int dmg) {
		data.health -= dmg;
        if (data.health <= 0)
            GameController.Instance.KillPlayer();
	}

	public void Attack() {
		RaycastHit2D[] results = new RaycastHit2D[1];
		int r = gameObject.GetComponent<Collider2D>().Cast(facing, results, punchDistance);
		if (r > 0 && results [0].rigidbody != null && results [0].rigidbody.gameObject.CompareTag("Enemy")) {
			results [0].rigidbody.gameObject.GetComponent<EnemyController>().TakeDamage(punchStrength);
		}
	}

	private const float sqrt2over2 = 0.707106781186f;

	private static Vector2 Snap(Vector2 v) {
		float abx = Mathf.Abs(v.x);
		float aby = Mathf.Abs(v.y);
		float sx = v.x == 0 ? 0 : Mathf.Sign(v.x);
		float sy = v.y == 0 ? 0 : Mathf.Sign(v.y);
		if (abx > 2 * aby) {
			return new Vector2(sx, 0);
		} else if (aby > 2 * abx) {
			return new Vector2(0, sy);
		} else {
			return new Vector2(sx * sqrt2over2, sy * sqrt2over2);
		}
	}
}
