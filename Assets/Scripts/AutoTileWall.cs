using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AutoTileWall : MonoBehaviour {

	enum Place {
		Empty,
		Floor,
		Wall}
	;

	enum Direction {
		Right,
		Down,
		Left,
		Up}
	;

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
			r = gameObject.GetComponent<Collider2D>().Raycast(dir, result, 
				gameObject.GetComponent<Collider2D>().bounds.size.x + 0.1f);
			if (r > 0) {
				if (result [0].collider.gameObject.CompareTag("Wall")) {
					// it sees a wall
					surrounding [i] = Place.Wall;
					//Debug.Log("Wall seen!");
				} else {
					// it sees a floor or door
					surrounding [i] = Place.Floor;
					//Debug.Log("Floor seen!");
				}
			}
		}
		switch (surrounding [Direction.Right]) {
		case 2: // wall to the right
			if (surrounding [Direction.Down] == Place.Wall) { // wall below, to the right
				if (surrounding [Direction.Left] == Place.Wall) { // 3 walls
					if (surrounding [Direction.Up] == Place.Floor) { // and a floor
						spr.sprite = wallD;
					} else {										// 4 walls
						spr.sprite = wallInner;
					}
				} else if (surrounding [Direction.Up] == Place.Wall) { // 3 walls
					if (surrounding [Direction.Left] == Place.Floor) {
						spr.sprite = wallL;
					} else {
						spr.sprite = wallInner;
					}
				} else { // exactly one wall below, one to the right
					if (surrounding [Direction.Left] == Place.Floor || surrounding [Direction.Up] == Place.Floor) { // floor facing corner
						spr.sprite = wallOutCornerDL;
					} else { // inner corner
						spr.sprite = wallCornerDL;
					}
				}
			} else if (surrounding [Direction.Left] == Place.Wall) { // wall to the left, to the right and not below
				if (surrounding [Direction.Up] == Place.Wall) { // wall to the left, to the right, and above
					if (surrounding [Direction.Down] == Place.Floor) { // floor below
						spr.sprite = wallD;
					} else { // 'nothing' below
						spr.sprite = wallInner;
					}
				} else { // wall to the left, to the right, and not below or above
					if (surrounding [Direction.Up] == Place.Floor) {
						if (surrounding [Direction.Down] == Place.Floor) {
							spr.sprite = wallHorInner; // floors on both sides
						} else {
							spr.sprite = wallU; 
						}
					} else if (surrounding [Direction.Down] == Place.Floor) {
						spr.sprite = wallD;
					} else {
						spr.sprite = wallInner; // should never come up, but if someone screwed up it's here
					}
				}
			} else if (surrounding [Direction.Up] == Place.Wall) { // wall above, to the right and not below or to the left
				if (surrounding [Direction.Down] == Place.Floor || surrounding [Direction.Left] == Place.Floor) {
					spr.sprite = wallOutCornerUL;
				} else {
					spr.sprite = wallCornerUL;
				}
			} else { // only wall is to the right
				spr.sprite = wallHorOuterR;
			}
			break;


		default: // empty to the right
			switch (surrounding [Direction.Down]) {
			case 2:
				if (surrounding [Direction.Left] == Place.Wall) { // wall below and to the left
					if (surrounding [Direction.Up] == Place.Wall) { // wall below, to the left, above
						if (surrounding [Direction.Right] == Place.Floor) {
							spr.sprite = wallL;
						} else {
							spr.sprite = wallInner;
						}
					} else if (surrounding [Direction.Up] == Place.Floor) { // floor above
						if (surrounding [Direction.Right] == Place.Floor) {
							spr.sprite = wallOutCornerDR;
						} else {
							spr.sprite = wallU;
						}
					} else { // empty space above
						if (surrounding [Direction.Right] == Place.Floor) { // fix this shit to work like the rest
							spr.sprite = wallL;
						} else {
							spr.sprite = wallCornerDR;
						}
					}
				} else if (surrounding [Direction.Up] == Place.Wall) { // wall below and above and not to the left
					if (surrounding [Direction.Left] == Place.Floor) {
						if (surrounding [Direction.Right] == Place.Floor) {
							spr.sprite = wallVertInner; // floors on both sides
						} else {
							spr.sprite = wallR; 
						}
					} else if (surrounding [Direction.Right] == Place.Floor) {
						spr.sprite = wallL;
					} else {
						spr.sprite = wallInner; // should never come up, but if someone screwed up it's here
					}
				} else { // just below
					spr.sprite = wallVertOuterU;
				}
				break;
			default: // empty to the right and below
				switch (surrounding [Direction.Left]) {
				case 2:
					if (surrounding [Direction.Up] == Place.Wall) {
						if (surrounding [Direction.Right] == Place.Floor || surrounding [Direction.Down] == Place.Floor) {
							spr.sprite = wallOutCornerUR;
						} else {
							spr.sprite = wallCornerUR;
						}
					} else if (surrounding [Direction.Up] == Place.Floor) {
						if (surrounding [Direction.Right] == Place.Floor) {
							if (surrounding [Direction.Down] == Place.Floor) {
								spr.sprite = wallHorOuterL;
							} else {
								spr.sprite = wallOutCornerDR;
							}
						} else {
							if (surrounding [Direction.Down] == Place.Floor) {
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
					switch (surrounding [Direction.Up]) {
					case 2:
						spr.sprite = wallVertOuterD;
						break;
					default: // no walls on any sides
						if (surrounding [Direction.Right] == Place.Floor ||
						    surrounding [Direction.Down] == Place.Floor ||
						    surrounding [Direction.Left] == Place.Floor ||
						    surrounding [Direction.Up] == Place.Floor) {
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
