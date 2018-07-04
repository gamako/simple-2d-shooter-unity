using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DrawGraph : MonoBehaviour 
{
    public int elemSize = 100;

    public class Item {
        public List<float> data;
        public Color color;
        public float max;
        public float min;

        public float rangeWidth;

        public Item(int size, Color color, float min = 0f, float max = 1f) {
            this.min = min;
            this.max = max;
            this.color = color;
            rangeWidth = max - min;
            data = new List<float>(size);
            for (int i = 0; i < size; i++) {
                data.Add(min);
            }
        }
    }

    Dictionary<string, Item> items = new Dictionary<string, Item>();

    public Rect area = Rect.zero;

    public void AddItem(string name, Color color, float min = 0f, float max = 1f) {
        items.Add(name, new Item(elemSize, color, min, max));
    }

    public void Remove(string name) {
        items.Remove(name);
    }

    // valueは
    public void Add(string name, float value) {
        var item = items[name];
        var v = value;
        items[name].data.Add(v);
        items[name].data.RemoveAt(0);
    }

    public void Start() {
        if (Rect.zero.Equals(area)) {
            area = new Rect(0, 0, Screen.width, Screen.height);
        }
    }

    void OnGUI() 
    {
        // Grid
        const int div = 10;
        for (int i = 0; i <= div; ++i) {
            var lineColor = (i == 0 || i == div) ? Color.white : Color.gray;
            var lineWidth = (i == 0 || i == div) ? 2f : 1f;
            var x = (area.width  / div) * i;
            var y = (area.height / div) * i;
            Drawing.DrawLine (
                new Vector2(area.x + x, area.y),
                new Vector2(area.x + x, area.yMax), lineColor, lineWidth, true);
            Drawing.DrawLine (
                new Vector2(area.x,    area.y + y),
                new Vector2(area.xMax, area.y + y), lineColor, lineWidth, true);
        }

        // Data
        foreach(KeyValuePair<string, Item> itemPair in items) {
            var item = itemPair.Value;
            var color = item.color;
            var data = item.data;
            var rangeWidth = item.rangeWidth;

            if (data.Count > 0) {
                var dx  = area.width / data.Count; 
                var dy  = area.height / rangeWidth;
                var previousPos = new Vector2(area.x, area.yMax - dy * (data[0] - item.min));
                for (var i = 1; i < data.Count; ++i) {
                    var x = area.x + dx * i;
                    var y = area.yMax - dy * (data[i] - item.min);
                    var currentPos = new Vector2(x, y);
                    Drawing.DrawLine(previousPos, currentPos, color, 3f, true);
                    previousPos = currentPos;
                }
            }
        }

    }
}