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

    float gravityLeft = -3;

    int shotCount = 0;
    SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite spritePlain;
    [SerializeField] private Sprite spriteSpeedUp;
    [SerializeField] private Sprite spriteTwinShot;
    [SerializeField] private Sprite spriteTripleShot;
    [SerializeField] private Sprite spriteOption;

    void Start() {
        isRolling = false;
        shotCount = 0;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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
        shotCount += 1;

        switch (shotCount) {
        case 5:
            spriteRenderer.sprite = spriteSpeedUp;
            break;
        case 10:
            spriteRenderer.sprite = spriteTwinShot;
            break;
        case 20:
            spriteRenderer.sprite = spriteTripleShot;
            break;
        case 30:
            spriteRenderer.sprite = spriteOption;
            break;
        default:
            Debug.Log($"spritePlain: {spritePlain}");
            spriteRenderer.sprite = spritePlain;
            break;
        }

    }

    void OnEndRolling() {
    }
}
