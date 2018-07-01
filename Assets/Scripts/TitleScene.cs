﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour {

    public GameObject PushButtonObject;

    AxisChange axis;

    // Use this for initialization
    void Start () {
        axis = new AxisChange("Fire1");
        cancellationTokenSource = new CancellationTokenSource();
        buttonOnce = false;

        GameSystemData.Instance.Score.Value = 0;
    }

    void OnDestroy() {
        cancellationTokenSource.Cancel();
    }

    // 一度しかボタンの処理を行わないためのフラグ
    bool buttonOnce;
    CancellationTokenSource cancellationTokenSource;

    // Update is called once per frame
    async void Update () {
        axis.Update();
        if (axis.ChangePositive) {
            if (buttonOnce) {
                return;
            }
            buttonOnce = true;
            GameObject o = GameObject.Find("PushAnyButton");
            if (o != null) {
                var flasher = o.GetComponent<FlashTextMeshProUGUI>();
                if (flasher != null) {
                    flasher.interval = 0.2f;

                    // メインスレッド2秒後に処理を行う
                    // タスクの終わりは待たない
                    await Task.Delay(2000);
                    SceneManager.LoadScene("GameScene");
                }
            }
        }
    }


}
