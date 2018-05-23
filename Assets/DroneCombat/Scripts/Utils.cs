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

    public static readonly float PHI = 1.61803398f;

    public static float Optimize(float x1, float x2, int iters, Func<float, float> cost) {
        float c = x2 - (x2 - x1) / PHI;
        float d = x1 + (x2 - x1) / PHI;
        for (int i=0; i < iters; i++) {
            if (cost(c) < cost(d)) {
                x2 = d;
            } else {
                x1 = c;
            }
            c = x2 - (x2 - x1) / PHI;
            d = x1 + (x2 - x1) / PHI;
        }
        return (x1 + x2) / 2;
    }

}
