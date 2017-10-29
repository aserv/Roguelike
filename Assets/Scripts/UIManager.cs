using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public UIBar healthBar;
    public UIBar expBar;
    public Image[] itemIcon = new Image[4];


    // Update is called once per frame
    void Update () {
        PlayerController.PlayerData data = GameController.Instance.data;
        if (data == null) return;
        healthBar.SetValue((float)data.health / (float)data.maxHealth);
        expBar.SetValue(data.exp / data.nextexp);
        for (int i = 0; i < 4; i++) {
            BaseItem it = data.items[i];
            if (it == null || it.Icon == null) {
                itemIcon[i].enabled = false;
            } else {
                itemIcon[i].enabled = true;
                itemIcon[i].sprite = it.Icon;
            }
        }
    }
}
