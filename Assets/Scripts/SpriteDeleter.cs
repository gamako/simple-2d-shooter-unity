using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// １秒以上画面内に戻ってこないオブジェクトを削除する
public class SpriteDeleter : MonoBehaviour {

    static float ttl = 1;

    // 一度でも画面に現れたもののみ対象とするためのフラグ
    // 一度も画面に現れないまま残ってしまうバグがあると困るので
    // 無効にしておく
    // bool isVisibleOnce = false;
    float lefeTimeMax = 0f;
	private Renderer[] renderers;

    void Start () {
        // this.isVisibleOnce = false;
		this.renderers = GetComponentsInChildren<Renderer>();
    }

    void Update () {
        bool isVisible = renderers.Aggregate(false, (acc, r) => { return r.isVisible || acc;  });

        // if (isVisibleOnce) {
            if (isVisible) {
                lefeTimeMax = Time.time + ttl;
            } else {
                if (lefeTimeMax < Time.time) {
                    Destroy(this.gameObject);
                }
            }
        // } else {
        //     if (isVisible) {
        //         isVisibleOnce = true;
        //     }
        // }
        
    }
}