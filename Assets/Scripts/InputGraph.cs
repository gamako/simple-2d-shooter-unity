using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputGraph : MonoBehaviour {

    DrawGraph graph;

    void Start () {
        graph = GetComponent<DrawGraph>();
        graph.area = new Rect(10, 10, Screen.width-20, Screen.height-20);

        graph.AddItem("Horizontal", Color.red, -4, 1);
        graph.AddItem("Vertical", Color.blue, -4, 1);
        graph.AddItem("HorizontalRaw", new Color(1.0f, 0.5f, 0.5f, 0.8f), -4, 1);
        graph.AddItem("VerticalRaw", new Color(0.5f, 0.8f, 1.0f, 0.8f), -4, 1);

        graph.AddItem("A", Color.cyan, -3, 10);
        graph.AddItem("D", Color.green, -3, 10);
        graph.AddItem("R", Color.cyan, 0, 7);
        graph.AddItem("L", Color.green, 0, 7);
    }
    
    void Update () {
        graph.Add("Horizontal", Input.GetAxis("Horizontal"));
        graph.Add("Vertical", Input.GetAxis("Vertical"));
        graph.Add("HorizontalRaw", Input.GetAxisRaw("Horizontal"));
        graph.Add("VerticalRaw", Input.GetAxisRaw("Vertical"));

        graph.Add("A",  Input.GetKey(KeyCode.A) ? 1 : 0);
        graph.Add("D",  Input.GetKey(KeyCode.D) ? 1 : 0);
        graph.Add("R",  Input.GetKey(KeyCode.RightArrow) ? 1 : 0);
        graph.Add("L",  Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
   }
}
