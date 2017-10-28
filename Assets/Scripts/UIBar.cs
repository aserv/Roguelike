using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour {
    public Image foreground;
    public Image background;

    private float value;
    public void SetValue(float portion) {
        portion = Mathf.Clamp(portion, 0, 1);
        Vector2 anchor = foreground.rectTransform.anchorMin;
        anchor.x = 1 - portion;
        foreground.rectTransform.anchorMin = anchor;
    }
}
