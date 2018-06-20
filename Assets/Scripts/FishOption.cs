using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FishOption : MonoBehaviour {

    List<Vector3> positions;
    public int positionNum;

    public Player PlayerObj;
    [SerializeField] private GameObject shotPrefab;

    void Start() {
        positions = new List<Vector3>();
        var p = PlayerObj.transform.localPosition;
        for (int i = 0; i < positionNum; i++) {
            positions.Add(p);
        }
    }

    void FixedUpdate() {
        var p = positions[positionNum-1];
        if (PlayerObj.transform.localPosition != p) {
            positions.Add(PlayerObj.transform.localPosition);
            var p1 = positions[0];
            positions.RemoveAt(0);
            gameObject.transform.localPosition = p1;
        }
    }

    void Update() {

    }

    public void shot() {
        var shotPosition = transform.localPosition;
        shotPosition.x += 0.7f;
        Instantiate(shotPrefab, shotPosition, Quaternion.identity);
    }

}