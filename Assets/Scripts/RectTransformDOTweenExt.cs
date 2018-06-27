using UnityEngine;
using DG.Tweening;

namespace MyExt {
    public static class RectTransformDOTweenExt {
        public static Tweener DOAnchorMinX(this RectTransform self, float x, float duration) {
            var m = self.anchorMin;
            m.x = x;
            return self.DOAnchorMin(m, duration);
        }
        public static Tweener DOAnchorMaxX(this RectTransform self, float x, float duration) {
            var m = self.anchorMax;
            m.x = x;
            return self.DOAnchorMax(m, duration);
        }
    }
}