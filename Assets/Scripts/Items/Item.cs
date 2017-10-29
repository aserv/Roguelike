using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseItem {
    public enum Result {
        Ignored,
        Consumed,
        Charging
    };
    public Sprite Icon { get; private set; }
    public string Name { get; private set; }
    public BaseItem(String name, Sprite icon) { Name = name; Icon = icon; }
    public virtual BaseItem Clone() { return (BaseItem)this.MemberwiseClone(); }
    public abstract Result Use(PlayerController player);
    public virtual Result Release(PlayerController player) { return Result.Ignored; }
}

namespace Items {
    public class HealthUpItem : BaseItem {
        public int HealthUp { get; private set; }
        public HealthUpItem(String name, Sprite icon) : base(name, icon) { HealthUp = 1; }
        public override Result Use(PlayerController player) {
            player.health += HealthUp;
            return Result.Consumed;
        }
    }
    public class BasicProjectileItem : BaseItem {
        public GameObject Prefab { get; private set; }
        public BasicProjectileItem(String name, Sprite icon, GameObject prefab) : base(name, icon) { Prefab = prefab; }
        public override Result Use(PlayerController player) {
            GameObject.Instantiate(Prefab, player.gameObject.transform.position, player.gameObject.transform.rotation);
            return Result.Consumed;
        }
    }
}
