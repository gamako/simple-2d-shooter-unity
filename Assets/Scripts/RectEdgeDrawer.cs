using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LineRendererでエッジ部分に枠線を書く
// LineRendererはエディタ上でセットしておくこと
[ExecuteInEditMode]
public class RectEdgeDrawer : MonoBehaviour {

    private LineRenderer lineRenderer;
    private RectTransform rectTransform;
    
    // Use this for initialization
    void Start () {
        this.rectTransform = gameObject.GetComponent<RectTransform>();
        this.lineRenderer = gameObject.GetComponent<LineRenderer>();
    }
    
    // Update is called once per frame
    void Update () {
        if (this.lineRenderer == null) {
            this.lineRenderer = gameObject.GetComponent<LineRenderer>();
        }
        var rect = rectTransform.rect;

        var topLeft = new Vector3(rect.xMin, rect.yMax);
        var topRight = new Vector3(rect.xMax, rect.yMax);
        var bottomLeft = new Vector3(rect.xMin, rect.yMin);
        var bottomRight = new Vector3(rect.xMax, rect.yMin);

        Vector3[] positions = new[] {topLeft, topRight, bottomRight, bottomLeft, topLeft};
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }
}
