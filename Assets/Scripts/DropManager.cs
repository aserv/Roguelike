using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DropManager : MonoBehaviour {
    public GameObject pickup;
    public GameObject[] prefabs;
    private ItemTable table = new ItemTable(1);

    void Start() {
        table.AddItem(new HealthUpItem("Mutton", null), 0, 10);
        table.AddItem(new BasicProjectileItem("Fireball", null, prefabs.First(x => x.name == "Fireball")), 0, 5);
    }

    public void Drop(Vector2 location) {
        BaseItem i = table.RandomDrop(0, 0);
        if (i == null) return;
        GameObject go = Instantiate(pickup, location, Quaternion.identity);
        go.GetComponent<Pickup>().item = i.Clone();
    }
}
