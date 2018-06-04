using UnityEngine;

// ボタンなどのAxisの変化したタイミングだけを取れるクラス
// Updateを自分で呼ぶ
public class AxisChange {
    public AxisChange(string name) {
        this.name = name;
    }
    string name;
    float time;
    float value;
    bool changePositive;
    bool changeNegative;
    bool changeZero;
    
    public void Update() {
        var currentTime = Time.time;
        if (time != currentTime) {
            time = currentTime;
            var oldValue = value;
            value = Input.GetAxis(name);
            changeZero = (oldValue != 0 && value == 0);
            changePositive = (oldValue < 0.5 && value >= 0.5);
            changeNegative = (oldValue > -0.5 && value <= -0.5);
        }
    }
    
    public bool ChangePositive {
        get { return changePositive; }
    }
    public bool ChangeNegative {
        get { return changeNegative; }
    }
    public bool ChangeZero {
        get { return changeZero; }
    }
}