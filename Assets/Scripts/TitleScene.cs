using System.Collections;
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
    }

    // Update is called once per frame
    void Update () {
        axis.Update();
        if (axis.ChangePositive) {
            GameObject o = GameObject.Find("PushAnyButton");
            if (o != null) {
                var flasher = o.GetComponent<FlashTextMeshProUGUI>();
                if (flasher != null) {
                    flasher.interval = 0.2f;

                    var context = SynchronizationContext.Current;

                    Debug.Log("ここはメインスレッド" + Thread.CurrentThread.ManagedThreadId);
                    
                    // Task.RunはTaskを返す
                    // 戻り値のTaskを捨てると、結果を待たずに捨てることを意味する
                    // asyncメソッドの中でawait Task.Run(...)とすると、async/awaitの非同期で結果待ちして、継続処理を行う
                    // Task.Waitを使うと同期的に待つ
                    // （UIスレッドで行うのであれば、UIスレッドを待たせることになるので使い所は限られる）
                    Task.Run(async() => {
                        Debug.Log("メインスレッド以外で実行" + Thread.CurrentThread.ManagedThreadId);
                        await Task.Delay(2000);
                        Debug.Log("メインスレッド以外で実行" + Thread.CurrentThread.ManagedThreadId);

                        // Unity側に触る場合はメインスレッドである必要がある
                        context.Post((state) => {
                            Debug.Log("メインスレッドで実行" + Thread.CurrentThread.ManagedThreadId);
                            SceneManager.LoadScene("GameScene");
                            
                            // 他の経路から遷移してしまう場合は、同時に起きないようにする必要があるだろう
                        }, null);
                    });
                }
            }
        }
    }


}
