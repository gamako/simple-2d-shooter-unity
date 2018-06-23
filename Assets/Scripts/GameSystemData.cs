using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameSystemData : MonoBehaviour {

    // Singletonとしてのアクセッサ
    public static GameSystemData Instance {
        get; private set;
    }
    
    public ReactiveProperty<int> HighScore;

    public ReactiveProperty<int> Score;
    
    
    void Awake() {
        // SIngltonとしての処理
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad (gameObject);

        Score = new ReactiveProperty<int>();
        HighScore = new ReactiveProperty<int>();

        LoadHighScore();
        // スコアが更新されたら必要に応じてハイスコアを更新
        Score.WithLatestFrom(HighScore, (s,h) =>  new {s,h} )
            .Where(v => v.s > v.h )
            .Subscribe(v => {
                HighScore.Value = v.s;
            })
            .AddTo(this);
        // ハイスコア更新されたら保存
        HighScore.Subscribe(_ => {
            SaveHighScore();
        }).AddTo(this);
    }
    
    void Update () {
    }

    static private string PrefsKeyHighScore = "HighScore";

    public void LoadHighScore() {
        var highScore = PlayerPrefs.GetInt(PrefsKeyHighScore, 0);
        HighScore.Value = highScore;
    }

    public void SaveHighScore() {
        PlayerPrefs.SetInt(PrefsKeyHighScore, HighScore.Value);
    }
}
