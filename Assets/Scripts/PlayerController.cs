using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
	public float moveSpeed;
	public int health;
    public int maxHealth;
	public int exp;
    public int nextexp;
	public int lvl;
	public float punchDistance;
	public int punchStrength;

	[SerializeField]
	private BaseItem[] items;
	private int nextItem = 0;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start() {
		items = new BaseItem[4];
		rb = this.GetComponent<Rigidbody2D>();
	}

	void Update() {
		if (items [0] != null && Input.GetButtonDown("Use1")) {
			UseItem(0);
		}
		if (items [1] != null && Input.GetButtonDown("Use2")) {
			UseItem(1);
		}
		if (items [2] != null && Input.GetButtonDown("Use3")) {
			UseItem(2);
		}
		if (items [3] != null && Input.GetButtonDown("Use4")) {
			UseItem(3);
		}
		if (Input.GetButtonDown("Attack")) {
			Attack();
		}
	}

	// Update is called once per frame
	void FixedUpdate() {
		Vector2 facing = Snap(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
		rb.velocity = facing * moveSpeed;
		if (facing != Vector2.zero) {
			rb.rotation = Mathf.Rad2Deg * Mathf.Atan2(facing.y, facing.x);
		}
	}

	public void AddExp(int xp) {
		exp += xp;
	}

	public bool PickupItem(BaseItem i) {
		if (nextItem == -1)
			return false;
		items [nextItem] = i;
		Debug.Log(i.Name);
		for (int n = 3; n >= 0; n--) {
			if (items [n] == null)
				nextItem = n;
		}
		return true;
	}

	public BaseItem getItemAt(int i) {
		return items [i];
	}

	private void UseItem(int i) {
		if (items [i] == null)
			return;
		BaseItem.Result res = items [i].Use(this);
		if (res == BaseItem.Result.Consumed) {
			if (nextItem == -1 || nextItem > i)
				nextItem = i;
			items [i] = null;
		}
	}

	public void TakeDamage(int dmg) {
		health -= dmg;
	}

	public void Attack() {
		RaycastHit2D[] results = new RaycastHit2D[1];
		int r = gameObject.GetComponent<Collider2D>().Cast(new Vector2(Mathf.Cos(Mathf.Deg2Rad * rb.rotation), Mathf.Sin(Mathf.Deg2Rad * rb.rotation)), results, punchDistance);
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
