using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public PlayerController player;
    public Text healthBar;
    public Text expBar;
    public Text itemsList;

    // Update is called once per frame
    void Update () {
        if (player == null) return;
        healthBar.text = String.Format("Health: {0}", player.health);
        expBar.text = String.Format("Lvl: {0} Exp: {1}", player.lvl, player.exp);
        String items = "";
        for (int i = 0; i < 4; i++) {
            BaseItem it = player.getItemAt(i);
            if (it == null) continue;
            items += String.Format("{0}\n", it.Name);
        }
        itemsList.text = items;
    }
}
