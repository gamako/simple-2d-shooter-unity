using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour {

	public Vector3 deltaVector = new Vector3(10, 0, 0); 

	private Renderer renderer;
	void Start () {
		renderer = GetComponent<Renderer>();
	}

	void Update () {
		var delta = Time.deltaTime;
		transform.localPosition += deltaVector * delta;

		if (!renderer.isVisible) {
    		Destroy(this.gameObject);
  		}
	}
}
