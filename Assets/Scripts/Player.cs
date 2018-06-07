using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    AxisChange fireAxis;

    static string horizontalAxis = "Horizontal";
    static string verticalAxis = "Vertical";

    public float Speed = 1f;

    public GameObject shotPrefab;

    void Start () {
        fireAxis = new AxisChange("Fire1");
    }

    void Update () {
        // プレイヤーの移動
        float delta = Time.deltaTime;
        var h = Input.GetAxis(horizontalAxis);
        var v = Input.GetAxis(verticalAxis);

        var old = transform.localPosition;
        transform.localPosition = new Vector3(
            old.x + h * delta * Speed,
            old.y + v * delta * Speed,
            old.z);

        // shot入力チェック
        fireAxis.Update();
        if (fireAxis.ChangePositive) {
            
            var shotPosition = transform.localPosition;
            shotPosition.x += 0.8f;
            shotPosition.y += 0.05f;
            // プレハブからインスタンスを生成
            Instantiate (shotPrefab, shotPosition, Quaternion.identity);
        }
        
    }


}
