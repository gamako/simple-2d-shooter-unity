using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public Vector3 delta = new Vector3(-1, 0);

    void Start () {
    }
    
    void Update () {
        var dletaTime = Time.deltaTime;

        transform.localPosition += delta * dletaTime;

        
    }
}
