using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DynamicMeshWithUGUI : Graphic
{
    // protected override void OnPopulateMesh(VertexHelper vh)
    // {
    //     Vector2 corner1 = Vector2.zero;
    //     Vector2 corner2 = Vector2.zero;

    //     corner1.x = 0f;
    //     corner1.y = 0f;
    //     corner2.x = 1f;
    //     corner2.y = 1f;

    //     corner1.x -= rectTransform.pivot.x;
    //     corner1.y -= rectTransform.pivot.y;
    //     corner2.x -= rectTransform.pivot.x;
    //     corner2.y -= rectTransform.pivot.y;

    //     corner1.x *= rectTransform.rect.width;
    //     corner1.y *= rectTransform.rect.height;
    //     corner2.x *= rectTransform.rect.width;
    //     corner2.y *= rectTransform.rect.height;

    //     vh.Clear();

    //     UIVertex vert = UIVertex.simpleVert;

    //     vert.position = new Vector2(corner1.x, corner1.y);
    //     vert.color = color;
    //     vh.AddVert(vert);

    //     Debug.Log($"corner1 {corner1}");

    //     vert.position = new Vector2(corner1.x, corner2.y);
    //     vert.color = color;
    //     vh.AddVert(vert);

    //     vert.position = new Vector2(corner2.x, corner2.y);
    //     vert.color = color;
    //     vh.AddVert(vert);

    //     vert.position = new Vector2(corner2.x, corner1.y);
    //     vert.color = color;
    //     vh.AddVert(vert);

    //     vh.AddTriangle(0, 1, 2);
    //     // vh.AddTriangle(2, 3, 0);
    // }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        Debug.Log("OnPopulateMesh");
        // 頂点の順番
        vh.AddTriangle(0,1,2);

        // // UIVertex:各頂点の情報
        var v0 = UIVertex.simpleVert;;
        v0.position = new Vector3(-100f, -100f);
        var v1 = UIVertex.simpleVert;;
        v1.position = new Vector3(0, 100f);
        var v2 = UIVertex.simpleVert;;
        v2.position = new Vector3(100f, -100f);

        // // 頂点情報を渡す
        vh.AddVert(v0);
        vh.AddVert(v1);
        vh.AddVert(v2);
    }
}