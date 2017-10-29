using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public PlayerController player;
    public UIBar healthBar;
    public UIBar expBar;
    public Image[] itemIcon = new Image[4];

    // Update is called once per frame
    void Update () {
        if (player == null) return;
        healthBar.SetValue(player.health / player.maxHealth);
        expBar.SetValue(player.exp / player.nextexp);
        for (int i = 0; i < 4; i++) {
            BaseItem it = player.GetItemAt(i);
            itemIcon[i].sprite = it == null ? null : it.Icon;
        }
    }
}
