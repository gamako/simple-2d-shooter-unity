using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBlock : MonoBehaviour {

    bool isRolling;
    float startTime = 0f;
    Matrix4x4 startMatrix;
    Vector2 size;
    float rollingDuration = 1;
    
    Vector3 rollingStartPosition;
    Vector3 speed;
    float rotationSpeed;

    void Start() {
        isRolling = false;
        //startRolling();
    }

    float gravityLeft = -3;

    void Update() {
        var delta = Time.deltaTime;
        var currentR = transform.localEulerAngles;

        speed.x = Mathf.Clamp(speed.x + delta * gravityLeft, -5, 5);
        var currentRz = currentR.z;
        if (rotationSpeed > 0) {
            rotationSpeed -= 1;
        } else if (rotationSpeed < 0) {
            rotationSpeed += 1;
        }

        transform.localPosition += speed * delta;
        var newR = currentR;
        newR.z += rotationSpeed * delta;
        transform.localRotation = Quaternion.Euler(newR);
    }

    void OnTriggerEnter2D(Collider2D other) {
    }

    public void OnDamaged(Collider2D other) {
        speed.x += 5;
        rotationSpeed += 90;
    }

    void OnEndRolling() {
    }
}
