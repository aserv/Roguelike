using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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

public class Pattern {
    public struct FireAction {
        public float Time;
        public Quaternion Direction;
        public bool Relative;
    }
    public Pattern(IEnumerable<FireAction> a) {
        this.actions = a.OrderBy(x => x.Time).ToArray();
    }
    private FireAction[] actions;
    public IEnumerator Begin(Action<Quaternion, bool> action) {
        int i = 0;
        float time = Time.time;
        while (i < actions.Length) {
            FireAction a = actions[i];
            if (actions[i].Time + time > Time.time) {
                yield return new WaitForSeconds(actions[i].Time + time - Time.time);
            } else {
                action(a.Direction, a.Relative);
                i++;
            }
        }
        yield break;
    }
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
        private Pattern pattern;
        public BasicProjectileItem(String name, Sprite icon, GameObject prefab, Pattern pattern) : base(name, icon) {
            Prefab = prefab;
            this.pattern = pattern;
        }
        public override Result Use(PlayerController player) {
            player.StartCoroutine(
                pattern.Begin((d, r) => {
                    Vector2 v = player.Facing();
                    Quaternion rot = r ? Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(v.y, v.x)) : Quaternion.identity;
                    rot *= d;
                    GameObject.Instantiate(Prefab, player.gameObject.transform.position, rot);
                })
            );
            return Result.Consumed;
        }
    }
}
