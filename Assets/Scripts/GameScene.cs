using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class GameScene : MonoBehaviour {

    private Camera mainCamera;
    private float nextEnemySpawnTime;

    public float enemySpawnInterval = 3;

    public GameObject enemyPrefab;

    [SerializeField] private GameObject player;

    // Use this for initialization
    void Start () {
        GameObject mainCameraObj = GameObject.Find ("Main Camera");
        mainCamera = mainCameraObj.GetComponent<Camera>();
        nextEnemySpawnTime = Time.time + enemySpawnInterval;
    }
    
    // Update is called once per frame
    void Update () {
        var t = Time.time;
        if (t > nextEnemySpawnTime) {
            nextEnemySpawnTime = t + enemySpawnInterval;

            // 敵キャラクターを画面端出現
            // var y = Random.Range(0, 1.0f); // y位置をランダムで決める
            // var p = mainCamera.ViewportToWorldPoint(new Vector2(1,y));
            // p.z = 0;
            // Instantiate (enemyPrefab, p, Quaternion.identity);
            spawnThree(3);

        }
    }

    void spawnThree(int count) {
        var context = SynchronizationContext.Current;

        var screenRightTop = mainCamera.ViewportToWorldPoint(new Vector2(1,1));
        var screenLeftBottom = mainCamera.ViewportToWorldPoint(new Vector2(0,0));

        Debug.Log($"screenRightTop: ${screenRightTop}");
        Debug.Log($"screenLeftBottom: ${screenLeftBottom}");

        var verticalCenter = (screenRightTop.y + screenLeftBottom.y)/2;

        float spawnY;
        if (player.transform.position.y < verticalCenter) {
            spawnY = screenLeftBottom.y + (screenRightTop.y - screenLeftBottom.y) * 0.8f;
        } else {
            spawnY = screenLeftBottom.y + (screenRightTop.y - screenLeftBottom.y) * 0.2f;
        }
        var spawnPoint = new Vector3(screenRightTop.x, spawnY, 0);

        Task.Run(async() => {
            for (int i = 0; i < count; i++) {
                context.Post((state) => {
                    Instantiate (enemyPrefab, spawnPoint, Quaternion.identity);

                }, null);
                await Task.Delay(700);
            } 
        });
    }
}
