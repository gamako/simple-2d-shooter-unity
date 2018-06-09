using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public GameObject particleObject;
	ParticleSystem particle;

    public Vector3 delta = new Vector3(-1, 0);

	private Renderer renderer_;

    void Start () {
		this.renderer_ = GetComponent<Renderer>();
		particle = particleObject.GetComponent<ParticleSystem>();
    }
    
    void Update () {
        var dletaTime = Time.deltaTime;

        // 画面外に出たら削除
        if (!renderer_.isVisible) {
    		Destroy(this.gameObject);
            return;
  		}

        // 移動
        transform.localPosition += delta * dletaTime;
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log("Enemy.OnTriggerEnter2D");
    }

    public void damaged(int num) {

        // パーティクルを発生
        particleObject.transform.localPosition = transform.localPosition;
        particle.Emit(200000);

        Destroy(this.gameObject);
    }
}
