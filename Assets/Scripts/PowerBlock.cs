using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBlock : MonoBehaviour {

    [SerializeField] private Camera mainCamera;

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

    float deadLine = 0f;

    [SerializeField] private Sprite spritePlain;
    [SerializeField] private Sprite spriteSpeedUp;
    [SerializeField] private Sprite spriteTwinShot;
    [SerializeField] private Sprite spriteTripleShot;
    [SerializeField] private Sprite spriteOption;

    public enum PoweUpType {
        Plain,
        SpeedUp,
        TwinShot,
        TripleShot,
        Option,
    }
    PoweUpType type_;

    public PoweUpType Type {
        get { return type_; }
    }


    void Start() {
        isRolling = false;
        shotCount = 0;

        spriteRenderer = GetComponent<SpriteRenderer>();
        type_ = PoweUpType.Plain;

        deadLine = mainCamera.ViewportToWorldPoint(new Vector2(0,0)).x - 1;
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

        // 画面左に行ったら消す
        if (transform.localPosition.x < deadLine) {
            Destroy(this.gameObject);
        }
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
            type_ = PoweUpType.SpeedUp;
            break;
        case 10:
            spriteRenderer.sprite = spriteTwinShot;
            type_ = PoweUpType.TwinShot;
            break;
        case 20:
            spriteRenderer.sprite = spriteTripleShot;
            type_ = PoweUpType.TripleShot;
            break;
        case 30:
            spriteRenderer.sprite = spriteOption;
            type_ = PoweUpType.Option;
            break;
        default:
            Debug.Log($"spritePlain: {spritePlain}");
            spriteRenderer.sprite = spritePlain;
            type_ = PoweUpType.Plain;
            break;
        }

    }

    void OnEndRolling() {
    }
}
