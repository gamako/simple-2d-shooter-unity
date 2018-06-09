﻿using System.Collections;
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

		if (!renderer_.isVisible) {
    		Destroy(this.gameObject);
  		}
	}
    void OnTriggerEnter2D(Collider2D other) {
        
		if (other.tag == "enemy") {
			Enemy e = other.GetComponent<Enemy>();
			if (e == null) {
				Debug.Log("can't find enemy component");
			}
			// 相手オブジェクトを削除し
			e.damaged(1);

			// パーティクルを発生
			particleObject.transform.localPosition = other.transform.localPosition;
			particle.Emit(10);

			// 自分も削除
			Destroy(this.gameObject);
		}
		
    }
}