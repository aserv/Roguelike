using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoTileWall : MonoBehaviour {

	public Sprite wallU;
	public Sprite wallD;
	public Sprite wallL;
	public Sprite wallR;

	public Sprite wallCornerUL;
	public Sprite wallCornerUR;
	public Sprite wallCornerDL;
	public Sprite wallCornerDR;

	public Sprite wallOutCornerUL;
	public Sprite wallOutCornerUR;
	public Sprite wallOutCornerDL;
	public Sprite wallOutCornerDR;

	public Sprite wallHorInner;
	public Sprite wallHorOuterL;
	public Sprite wallHorOuterR;

	public Sprite wallVertInner;
	public Sprite wallVertOuterU;
	public Sprite wallVertOuterD;

	public Sprite wallInner;
	public Sprite wallLone;


	private SpriteRenderer spr;

	// Use this for initialization
	void Start() {
		spr = gameObject.GetComponent<SpriteRenderer>();
		int[] surrounding = new int[4]{ 0, 0, 0, 0 };
		RaycastHit2D[] result = new RaycastHit2D [1];
		Vector2 dir;
		int r;
		// get a direction from r, update array with for loop

		for (int i = 0; i < 4; i++) {
			dir = new Vector2(Mathf.Cos(Mathf.PI * i / 2), Mathf.Sin(Mathf.PI * i / 2));
			//Debug.Log(dir);
			r = gameObject.GetComponent<Collider2D>().Raycast(dir, result, gameObject.GetComponent<Collider2D>().bounds.size.x + 0.1f);
			if (r > 0) {
				if (result [0].collider.gameObject.CompareTag("Wall")) {
					// it sees a wall
					surrounding [i] = 2;
					//Debug.Log("Wall seen!");
				} else {
					// it sees a floor or door
					surrounding [i] = 1;
					//Debug.Log("Floor seen!");
				}
			}
		}
		switch (surrounding [0]) {
		case 2: // wall to the right
			if (surrounding [1] == 2) { // wall below, to the right
				if (surrounding [2] == 2) { // 3 walls
					if (surrounding [3] == 1) { // and a floor
						spr.sprite = wallD;
					} else {										// 4 walls
						spr.sprite = wallInner;
					}
				} else if (surrounding [3] == 2) { // 3 walls
					if (surrounding [2] == 1) {
						spr.sprite = wallL;
					} else {
						spr.sprite = wallInner;
					}
				} else { // exactly one wall below, one to the right
					if (surrounding [2] == 1 || surrounding [3] == 1) { // floor facing corner
						spr.sprite = wallOutCornerDL;
					} else { // inner corner
						spr.sprite = wallCornerDL;
					}
				}
			} else if (surrounding [2] == 2) { // wall to the left, to the right and not below
				if (surrounding [3] == 2) { // wall to the left, to the right, and above
					if (surrounding [1] == 1) { // floor below
						spr.sprite = wallD;
					} else { // 'nothing' below
						spr.sprite = wallInner;
					}
				} else { // wall to the left, to the right, and not below or above
					if (surrounding [3] == 1) {
						if (surrounding [1] == 1) {
							spr.sprite = wallHorInner; // floors on both sides
						} else {
							spr.sprite = wallU; 
						}
					} else if (surrounding [1] == 1) {
						spr.sprite = wallD;
					} else {
						spr.sprite = wallInner; // should never come up, but if someone screwed up it's here
					}
				}
			} else if (surrounding [3] == 2) { // wall above, to the right and not below or to the left
				if (surrounding [1] == 1 || surrounding [2] == 1) {
					spr.sprite = wallOutCornerUL;
				} else {
					spr.sprite = wallCornerUL;
				}
			} else { // only wall is to the right
				spr.sprite = wallHorOuterR;
			}
			break;


		default: // empty to the right
			switch (surrounding [1]) {
			case 2:
				if (surrounding [2] == 2) { // wall below and to the left
					if (surrounding [3] == 2) { // wall below, to the left, above
						if (surrounding [0] == 1) {
							spr.sprite = wallL;
						} else {
							spr.sprite = wallInner;
						}
					} else if (surrounding [3] == 1) { // floor above
						if (surrounding [0] == 1) {
							spr.sprite = wallOutCornerDR;
						} else {
							spr.sprite = wallU;
						}
					} else { // empty space above
						if (surrounding [0] == 1) { // fix this shit to work like the rest
							spr.sprite = wallL;
						} else {
							spr.sprite = wallCornerDR;
						}
					}
				} else if (surrounding [3] == 2) { // wall below and above and not to the left
					if (surrounding [2] == 1) {
						if (surrounding [0] == 1) {
							spr.sprite = wallVertInner; // floors on both sides
						} else {
							spr.sprite = wallR; 
						}
					} else if (surrounding [0] == 1) {
						spr.sprite = wallL;
					} else {
						spr.sprite = wallInner; // should never come up, but if someone screwed up it's here
					}
				} else { // just below
					spr.sprite = wallVertOuterU;
				}
				break;
			default: // empty to the right and below
				switch (surrounding [2]) {
				case 2:
					if (surrounding [3] == 2) {
						if (surrounding [0] == 1 || surrounding [1] == 1) {
							spr.sprite = wallOutCornerUR;
						} else {
							spr.sprite = wallCornerUR;
						}
					} else if (surrounding [3] == 1) {
						if (surrounding [0] == 1) {
							if (surrounding [1] == 1) {
								spr.sprite = wallHorOuterL;
							} else {
								spr.sprite = wallOutCornerDR;
							}
						} else {
							if (surrounding [1] == 1) {
								spr.sprite = wallInner;
							} else {
								spr.sprite = wallU;
							}
						}
					} else {
						spr.sprite = wallInner;
					}
					break;
				default: // empty to the right, left, and below
					switch (surrounding [3]) {
					case 2:
						spr.sprite = wallVertOuterD;
						break;
					default: // no walls on any sides
						if (surrounding [0] == 1 || surrounding [1] == 1 || surrounding [2] == 1 || surrounding [3] == 1) {
							spr.sprite = wallLone;
						} else {
							spr.sprite = wallInner;
						}
						break;
					}
					break;
				}
				break;
			}
			break;
		}

	}
}
