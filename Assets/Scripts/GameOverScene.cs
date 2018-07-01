using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using MyExt;

public class GameOverScene : MonoBehaviour {

    AxisChange fireAxis;
    CancellationTokenSource cancellationTokenSource;

    async void Start () {
        fireAxis = new AxisChange("Fire1");
        cancellationTokenSource = new CancellationTokenSource();

        var context = SynchronizationContext.Current;
        var token = cancellationTokenSource.Token;

        try {
            await AwaitHelper.Delay(21f, token);
            SceneManager.LoadScene("TitleScene");
        } catch (TaskCanceledException) {
        }
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
