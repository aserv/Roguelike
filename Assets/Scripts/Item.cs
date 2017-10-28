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
    public string Name { get; private set; }
    public BaseItem(String name) { Name = name; }
    public abstract Result Use(PlayerController player);
    public virtual Result Release(PlayerController player) { return Result.Ignored; }
}

public class HealthUpItem : BaseItem {
    public int HealthUp { get; private set; }
    public HealthUpItem() : base("MedPack") { HealthUp = 1; }
    public override Result Use(PlayerController player) {
        player.health += HealthUp;
        return Result.Consumed;
    }
}
