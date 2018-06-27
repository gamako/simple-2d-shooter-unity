using UnityEngine;

namespace MyExt {
    public static class RectTransformExt
    {
        public static void setAnchorRangeX(this RectTransform self, float min, float max) {
            var a = self.anchorMin;
            a.x = min;
            self.anchorMin = a;
            a = self.anchorMax;
            a.x = max;
            self.anchorMax = a;
        }
    }
}