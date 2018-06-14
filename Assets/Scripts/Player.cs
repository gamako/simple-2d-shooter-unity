using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    AxisChange fireAxis;

    static string horizontalAxis = "Horizontal";
    static string verticalAxis = "Vertical";

    public float Speed = 1f;

    public GameObject shotPrefab;

    [SerializeField] private GameObject mainCameraObj;
    private Camera mainCamera;

    void Start () {
        fireAxis = new AxisChange("Fire1");
        mainCamera = mainCameraObj.GetComponent<Camera>();
    }

    void Update () {
        // プレイヤーの移動
        float delta = Time.deltaTime;
        var h = Input.GetAxis(horizontalAxis);
        var v = Input.GetAxis(verticalAxis);

        var old = transform.localPosition;
        var newPosition = new Vector3(
            old.x + h * delta * Speed,
            old.y + v * delta * Speed,
            old.z);

        // 画面端に出ないようにする
        var topLeft = mainCamera.ViewportToWorldPoint(new Vector2(0,0));
        var bottomRight = mainCamera.ViewportToWorldPoint(new Vector2(1,1));
        newPosition.x = Mathf.Clamp(newPosition.x, topLeft.x, bottomRight.x);
        newPosition.y = Mathf.Clamp(newPosition.y, topLeft.y, bottomRight.y);
        
        transform.localPosition = newPosition;

        // shot入力チェック
        fireAxis.Update();
        if (fireAxis.ChangePositive) {
            var c = GetComponent<Animator>();
            c.SetTrigger("shot-trig");
            c.SetBool("shot", true);
            var shotPosition = transform.localPosition;
            shotPosition.x += 0.8f;
            shotPosition.y += 0.05f;
            // プレハブからインスタンスを生成
            Instantiate (shotPrefab, shotPosition, Quaternion.identity);
        }
        
    }


}
