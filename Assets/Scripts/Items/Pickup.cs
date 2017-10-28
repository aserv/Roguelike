using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Pickup : MonoBehaviour {
    public GameObject fireball;
    private void Start() {
        //item = new HealthUpItem("MedPack");
        item = new BasicProjectileItem("Fireball", fireball);
    }

    public BaseItem item;
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            if (collider.GetComponent<PlayerController>().PickupItem(item)) {
                Destroy(gameObject);
            }
        }
    }
}
