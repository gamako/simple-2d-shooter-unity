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

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject powerUpPrefab;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject levelTextObj;
    private TextMeshProUGUI levelText;

    private List<CancellationTokenSource> cancelOnDisable = new List<CancellationTokenSource>();

    int level;

    // Use this for initialization
    async void Start () {
        GameObject mainCameraObj = GameObject.Find ("Main Camera");
        mainCamera = mainCameraObj.GetComponent<Camera>();
        levelText = levelTextObj.GetComponent<TextMeshProUGUI>();

        startPlayerStartDemo();

        level = 0;
        while (true) {
            try {
                level += 1;

                startLevelStartDemo();

                await levelSystem();

            } catch (TaskCanceledException) {
                // 途中でシーンが変わったり、エディタに戻ったりした場合はキャンセルで抜けてくる
                break;
            }
        }
    }
    
    void OnDisable() {
        // Cancel中にリスト(cancelOnDisable)を更新することがあってはならない
        cancelOnDisable.ForEach((c) => {
            c.Cancel();
        });
        cancelOnDisable.Clear();
    }

    // Update is called once per frame
    void Update () {
    }

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
        levelText.text = $"LEVEL {level}";
        
        levelText.rectTransform.setAnchorRangeX(1.5f,  2.5f);

        // レベル表示が右から出てきて左に去って行く動き
        DOTween.Sequence()
            .Append(levelText.rectTransform.DOAnchorMinX(0, 0.5f).SetEase(Ease.OutCubic))
            .Join(levelText.rectTransform.DOAnchorMaxX(1, 0.5f).SetEase(Ease.OutCubic))
            .AppendInterval(0.5f)
            .Append(levelText.rectTransform.DOAnchorMinX(-1, 0.5f).SetEase(Ease.InCubic))
            .Join(levelText.rectTransform.DOAnchorMaxX(0, 0.5f).SetEase(Ease.InCubic));
    }

    async Task levelSystem() {

        int currentLevel = this.level;
        Debug.Log($"level {currentLevel}");

        var cancelation = new CancellationTokenSource();
        cancelOnDisable.Add(cancelation);

        // パワーアップアイテムを定期的に出現させる（終わりを待たない）
        spawnPowerUpCyclically(1f, 5f, cancelation.Token);

        // 敵を倒すなどのイベントを待つときはTaskCompletionSourceを待つことで実現できる
        // var tcs = new TaskCompletionSource<bool>();
        // Task<bool> t = tcs.Task;
        // 
        // await Task.Run(async() => {
        //     await Task.Delay(2000);
        //     tcs.SetResult(true);
        // });
        // var r = await t;
        // await spawnEnemySerialCyclically(level + 1, level, 2, 2, cancelation.Token);
        await levelEnemy(cancelation.Token);

        await Task.Delay(TimeSpan.FromMilliseconds(1000 * 5), cancelation.Token);

        // 終わりを待たないタスクを終了させる
        cancelation.Cancel();
        cancelOnDisable.Remove(cancelation);
    }

    async Task levelEnemy(CancellationToken token) {
        var l = level % 2;
        Debug.Log("l " + l);
        switch(l) {
        case 0:
            await level2Enemy(token);
            break;
        case 1:
            await level1Enemy(token);
            break;
        }
    }

    async Task level1Enemy(CancellationToken token) {
        int count = 3;
        int roundCount = 2 + level;
        float startDelay = 3;
        float interval = 3;
        float speed = (float)level * 0.5f + 1.0f;

        await TaskExt.Delay(startDelay, token);
        for (int i = 0; i < roundCount; i++) {
            await spawnEnemy1Serial(count, speed, token);
            await TaskExt.Delay(interval, token);
        }
    }

    async Task level2Enemy(CancellationToken token) {
        int count = 2 + level;
        int roundCount =  2 + level;
        float startDelay = 3;
        float interval = 3;
        float speed = (float)level * 0.5f + 1.0f;

        await TaskExt.Delay(startDelay, token);
        for (int i = 0; i < roundCount; i++) {
            await spawnEnemy2Serial(count, speed, token);
            await TaskExt.Delay(interval, token);
        }
    }

    async Task spawnEnemy1Serial(int count, float speed, CancellationToken token) {
        var verticalCenter = mainCamera.ViewportToWorldPoint(new Vector2(1,0.5f)).y;

        Vector3 spawnPoint;
        if (player.transform.position.y < verticalCenter) {
            spawnPoint = mainCamera.ViewportToWorldPoint(new Vector2(1,0.8f));
        } else {
            spawnPoint = mainCamera.ViewportToWorldPoint(new Vector2(1,0.2f));
        }
        spawnPoint.z = 0;

        for (int i = 0; i < count; i++) {
            var enemy_ = Instantiate (enemyPrefab, spawnPoint, Quaternion.identity);
            var enemy = enemy_.GetComponent<Enemy>();
            enemy.Delta *= speed;
            
            float delayTime = 0.7f / speed;
            await TaskExt.Delay(delayTime, token);
        }
    }
    async Task spawnEnemy2Serial(int count, float speed, CancellationToken token) {

        for (int i = 0; i < count; i++) {
            var y = UnityEngine.Random.Range(0, 1.0f);
            Vector3 spawnPoint = mainCamera.ViewportToWorldPoint(new Vector2(1,y));
            spawnPoint.z = 0;

            var enemy_ = Instantiate (enemyPrefab, spawnPoint, Quaternion.identity);
            var enemy = enemy_.GetComponent<Enemy>();

            Vector3 delta = new Vector3(-3, 0);
            if (player.transform.position.y > spawnPoint.y) {
                delta = Quaternion.Euler(0, 0, -25) * delta;
            } else {
                delta = Quaternion.Euler(0, 0, 25) * delta;
            }
            enemy.Delta = delta * speed;

            float delayTime = 0.7f / speed;
            await TaskExt.Delay(delayTime, token);
        }
    }

    // パワーアップアイテムを定期的に出現させる非同期メソッド
    async Task spawnPowerUpCyclically(float startDelay, float intervalSec, CancellationToken token) {
        await Task.Delay(TimeSpan.FromMilliseconds(1000 * startDelay), token);

        while(true) {
            spawnPowerUp();
            await Task.Delay(TimeSpan.FromMilliseconds(1000 * intervalSec), token);
        }
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
