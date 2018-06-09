using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour {

    private Camera mainCamera;
    private float nextEnemySpawnTime;

    public float enemySpawnInterval = 2;

    public GameObject enemyPrefab;
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
            var y = Random.Range(0, 1.0f); // y位置をランダムで決める
            var p = mainCamera.ViewportToWorldPoint(new Vector2(1,y));
            p.z = 0;
            Instantiate (enemyPrefab, p, Quaternion.identity);


        }
    }
}
