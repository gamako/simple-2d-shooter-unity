using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBlock : MonoBehaviour {

    bool isRolling;
    float startTime = 0f;
    Matrix4x4 startMatrix;
    Vector2 size;
    float rollingDuration = 1;
    
    Vector3 rollingStartPosition;

    void Start() {
        isRolling = false;
        //startRolling();
    }

    void Update() {

        if (isRolling) {
            float t = (Time.time - startTime) / rollingDuration;

            if (t > 1) {
                t = 1;
            }
            t = EasingLerps.EasingLerp(EasingLerps.EasingLerpsType.Back, EasingLerps.EasingInOutType.EaseOut, t, 0, 1);
            
            // 右下を中心に回転する
            var m = 
            Matrix4x4.Translate(new Vector3(rollingStartPosition.x + size.x/2, rollingStartPosition.y - size.y/2, 0))
            * Matrix4x4.Rotate(Quaternion.Euler(0,0,-90f * t))
            * Matrix4x4.Translate(new Vector3(-rollingStartPosition.x - size.x/2, -rollingStartPosition.y + size.y/2, 0))
            * startMatrix;



            transform.localPosition = m.ExtractPosition();
            transform.localRotation = m.ExtractRotation();
            transform.localScale = m.ExtractScale();

            if (t == 1) {
                isRolling = false;
                OnEndRolling();
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other) {
    }

    public void OnDamaged(Collider2D other) {
        if (isRolling) {
            rollingDuration *= 0.8f;
        } else {
            startRolling();
        }
    }

    void startRolling() {
        startTime = Time.time;
        var col = GetComponent<BoxCollider2D>();
        size = col.size;

        // 回転の初期状態を表す行列
        startMatrix = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);

        isRolling = true;
        rollingDuration = 0.5f;

        rollingStartPosition = transform.localPosition;
    }

    void OnEndRolling() {
    }
}
