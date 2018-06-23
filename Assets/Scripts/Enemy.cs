using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour {

	public GameObject particleObject;
	ParticleSystem particle;

    public Vector3 delta = new Vector3(-3, 0);

	private Renderer[] renderers;

    [SerializeField] private GameObject soundEffectObj;
    private SoundEffect soundEffect;
    
    void Start () {
		this.renderers = GetComponentsInChildren<Renderer>();
		particle = particleObject.GetComponent<ParticleSystem>();
        soundEffect = soundEffectObj.GetComponent<SoundEffect>();
    }
    
    void Update () {
        var dletaTime = Time.deltaTime;

        // 移動
        transform.localPosition += delta * dletaTime;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            var p = other.GetComponent<Player>();
            p.OnDamage();
        }
    }

    public void damaged(int num) {

        // パーティクルを発生
        // Emitとposition変更の順番を逆にするとうまくいかない
        particle.Emit(100);
        particleObject.transform.localPosition = transform.localPosition;

        Destroy(this.gameObject);
        soundEffect.playBomb();
    }
}
