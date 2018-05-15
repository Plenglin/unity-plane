using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {

    public static IEnumerable<Transform> AllChildrenOf(Transform tp) {
        yield return tp;
        for (int i=0; i < tp.childCount; i++) {
            Transform tc = tp.GetChild(i);
            yield return tc;
            foreach (Transform t in AllChildrenOf(tc)) {
                yield return t;
            }
        }
    }

    public static float Optimize(float x1, float x2, int iters, Func<float, float> cost) {
        float y1, y2;
        for (int i = 0; i < iters; i++) {
            y1 = cost(x1);
            y2 = cost(x2);
            float xNext = (x1 + x2) / 2;
            if (y1 < y2) {
                x2 = xNext;
            } else {
                x1 = xNext;
            }
        }
        return (x1 + x2) / 2;
    }

}
