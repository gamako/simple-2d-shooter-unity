using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour {

	public GameObject particleObject;
	ParticleSystem particle;

    public Vector3 delta = new Vector3(-1, 0);

	private Renderer[] renderers;

    void Start () {
		this.renderers = GetComponentsInChildren<Renderer>();
		particle = particleObject.GetComponent<ParticleSystem>();
    }
    
    void Update () {
        var dletaTime = Time.deltaTime;

        // 移動
        transform.localPosition += delta * dletaTime;
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log("Enemy.OnTriggerEnter2D");
    }

    public void damaged(int num) {

        // パーティクルを発生
        // Emitとposition変更の順番を逆にするとうまくいかない
        particle.Emit(100);
        particleObject.transform.localPosition = transform.localPosition;

        Destroy(this.gameObject);
    }
}
