using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

	public GameObject particleObject;
	ParticleSystem particle;
	public Vector3 deltaVector = new Vector3(10, 0, 0); 

	private Renderer renderer_;
	void Start () {
		renderer_ = GetComponent<Renderer>();
		particle = particleObject.GetComponent<ParticleSystem>();
	}

	void Update () {
		var delta = Time.deltaTime;
		transform.localPosition += deltaVector * delta;
	}
    void OnTriggerEnter2D(Collider2D other) {
        
		if (other.tag == "enemy") {
			Enemy e = other.GetComponent<Enemy>();
			if (e == null) {
				Debug.Log("can't find enemy component");
			}
			// 相手オブジェクトを削除し
			e.damaged(1);

			// 自分も削除
			Destroy(this.gameObject);

			GameSystemData.Instance.Score.Value += 100;

		} else if (other.tag == "PowerUp") {

			PowerBlock p = other.GetComponent<PowerBlock>();
			p.OnDamaged(this.GetComponent<Collider2D>());
			Destroy(this.gameObject);
		}
		
    }
}
