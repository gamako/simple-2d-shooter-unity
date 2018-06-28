using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using DG.Tweening;
using UniRx;
using MyExt;

public class GameScene : MonoBehaviour {

    private Camera mainCamera;
    private float nextEnemySpawnTime;
    private float nextPowerUpSpawnTime;

    public float enemySpawnInterval = 3;
    public float powerUpSpawnInterval = 4;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject powerUpPrefab;

    [SerializeField] private GameObject player;

    CancellationTokenSource cancellationTokenSource;

    // Use this for initialization
    void Start () {
        GameObject mainCameraObj = GameObject.Find ("Main Camera");
        mainCamera = mainCameraObj.GetComponent<Camera>();
        levelText = levelTextObj.GetComponent<TextMeshProUGUI>();

        nextEnemySpawnTime = Time.time + enemySpawnInterval;
        nextPowerUpSpawnTime = Time.time + powerUpSpawnInterval;

        // シーンが遷移してしまったときに非同期タスクを終了するためのtoken
        cancellationTokenSource = new CancellationTokenSource();

        startPlayerStartDemo();
        startLevelStartDemo();
    }
    
    void OnDisable() {
        cancellationTokenSource.Cancel();
    }

    // Update is called once per frame
    void Update () {
        var t = Time.time;
        if (t > nextEnemySpawnTime) {
            nextEnemySpawnTime = t + enemySpawnInterval;
            spawnThree(3);
        }
        if (t > nextPowerUpSpawnTime) {
            nextPowerUpSpawnTime = t + powerUpSpawnInterval;
            spawnPowerUp();
        }
    }
    [SerializeField] private GameObject levelTextObj;
    private TextMeshProUGUI levelText;

    void startPlayerStartDemo() {
        // プレイヤー初期位置
        var starPosition = mainCamera.ViewportToWorldPoint(new Vector2(-0.2f, 0.5f));
        player.transform.position = new Vector3(starPosition.x, starPosition.y, 0);
        // プレイヤーが左から出てくる動き
        player.transform.DOMoveX(0, 1f);
    }

    void startLevelStartDemo() {
        levelText.enabled = true;
        levelText.color = Color.white;
        
        levelText.rectTransform.setAnchorRangeX(1.5f,  2.5f);

        // レベル表示が右から出てきて左に去って行く動き
        DOTween.Sequence()
            .Append(levelText.rectTransform.DOAnchorMinX(0, 0.5f).SetEase(Ease.OutCubic))
            .Join(levelText.rectTransform.DOAnchorMaxX(1, 0.5f).SetEase(Ease.OutCubic))
            .AppendInterval(0.5f)
            .Append(levelText.rectTransform.DOAnchorMinX(-1, 0.5f).SetEase(Ease.InCubic))
            .Join(levelText.rectTransform.DOAnchorMaxX(0, 0.5f).SetEase(Ease.InCubic));
    }

    void spawnThree(int count) {
        var context = SynchronizationContext.Current;

        var screenRightTop = mainCamera.ViewportToWorldPoint(new Vector2(1,1));
        var screenLeftBottom = mainCamera.ViewportToWorldPoint(new Vector2(0,0));

        var verticalCenter = (screenRightTop.y + screenLeftBottom.y)/2;

        float spawnY;
        if (player.transform.position.y < verticalCenter) {
            spawnY = screenLeftBottom.y + (screenRightTop.y - screenLeftBottom.y) * 0.8f;
        } else {
            spawnY = screenLeftBottom.y + (screenRightTop.y - screenLeftBottom.y) * 0.2f;
        }
        var spawnPoint = new Vector3(screenRightTop.x, spawnY, 0);
        var token = cancellationTokenSource.Token;
        Task.Run(async() => {
            for (int i = 0; i < count; i++) {
                if (token.IsCancellationRequested) {
                    return;
                }
                context.Post((state) => {
                    if (token.IsCancellationRequested) {
                        return;
                    }
                    Instantiate (enemyPrefab, spawnPoint, Quaternion.identity);
                }, null);
                await Task.Delay(700);
            } 
        });
    }

    void spawnPowerUp() {
        var screenRightTop = mainCamera.ViewportToWorldPoint(new Vector2(1,1));
        var screenLeftBottom = mainCamera.ViewportToWorldPoint(new Vector2(0,0));

        var y = UnityEngine.Random.Range(0, 1.0f);
        var spawnPoint = mainCamera.ViewportToWorldPoint(new Vector2(1,y));
        spawnPoint.z = 0;
        Instantiate (powerUpPrefab, spawnPoint, Quaternion.identity);
         
    }
}
