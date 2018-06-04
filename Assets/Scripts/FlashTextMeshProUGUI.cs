using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashTextMeshProUGUI : MonoBehaviour {

    public float interval = 0.5f;
    float nextTime;
    TextMeshProUGUI textmeshPro;
    // Use this for initialization
    void Start () {
        if (interval <= 0f) {
            interval = 0.5f;
            nextTime = interval;
        }
        textmeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update () {
        var current = Time.time;
        if (current > nextTime) {
            nextTime = current + interval;

            var color = textmeshPro.faceColor;

            byte a;
            if (color.a < 128) {
                a = 255;
            } else {
                a = 0;
            }
            textmeshPro.faceColor = new Color32(
                color.r,
                color.g,
                color.b,
                a);
        }
	}
}
