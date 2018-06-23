using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour {

    AxisChange fireAxis;
    CancellationTokenSource cancellationTokenSource;

    void Start () {
        fireAxis = new AxisChange("Fire1");
        cancellationTokenSource = new CancellationTokenSource();

        var context = SynchronizationContext.Current;
        var token = cancellationTokenSource.Token;

        Task.Run(async() => {
            await Task.Delay(21000);
            if (token.IsCancellationRequested) {
                return;
            }
            context.Post((state) => {
                if (token.IsCancellationRequested) {
                    return;
                }
                SceneManager.LoadScene("TitleScene");
            }, null);
        });
    }
    
    void OnDisable() {
        cancellationTokenSource.Cancel();
    }

    void Update () {
        fireAxis.Update();
        if (fireAxis.ChangePositive) {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
