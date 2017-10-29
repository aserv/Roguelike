using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


	public float leftBound;
	public float rightBound;
	public float upBound;
	public float downBound;
	public float lerpPercent;

	private PlayerController player;
	private float size;
	private float z;
	// Use this for initialization
	void Start() {
		player = GameObject.FindObjectOfType<PlayerController>();
		size = gameObject.GetComponent<Camera>().orthographicSize;
		z = gameObject.transform.position.z;
		gameObject.transform.position = new Vector3(player.gameObject.transform.position.x, player.gameObject.transform.position.y, z);
	}
	
	// Update is called once per frame
	void Update() {
		Vector2 lerp = Vector2.Lerp(gameObject.transform.position, player.transform.position, lerpPercent);
		Vector3 pos = new Vector3(lerp.x, lerp.y, z);
		gameObject.transform.position = pos;
	}
}