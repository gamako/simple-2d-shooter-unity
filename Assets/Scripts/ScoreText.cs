using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

public class ScoreText : MonoBehaviour {

    void Start () {
        var data = GameSystemData.Instance;
        var text = GetComponent<TextMeshProUGUI>();

        data.Score
            .CombineLatest(data.HighScore, (s, h) => new {s,h})
            .Subscribe(t => {
                var s = t.s.ToString();
                var h = t.h.ToString();
                text.text = $"SCORE {s}    HIGH-SCORE {h}";
            }).AddTo(this);
    }

    void Update () {
        
    }
}
