using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour {

    // Use this for initialization
    void Start () {
        var context = SynchronizationContext.Current;

        Task.Run(async() => {
            await Task.Delay(2000);
            context.Post((state) => {
                SceneManager.LoadScene("TitleScene");
            }, null);
        });
    }
    
    // Update is called once per frame
    void Update () {
        
    }
}
