using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DroneCombat.Combat {

    public class Tracer : MonoBehaviour {

        public Vector3 hit;
        public float duration;
        public Color color;
        public AnimationCurve alphaCurve;

        private LineRenderer line;
        private float start, end;

        // Use this for initialization
        private void Start() {
            line = GetComponent<LineRenderer>();
            line.useWorldSpace = true;
            start = Time.time;
            end = start + duration;
            line.SetPosition(0, transform.position);
        }

        // Update is called once per frame
        private void Update() {
            color.a = alphaCurve.Evaluate((Time.time - start) / duration);
            line.SetPosition(1, hit);
            line.startColor = color;
            line.endColor = color;
            if (Time.time > end) {
                Destroy(gameObject);
            }
        }

        public void PlaceAt(Vector3 start, Vector3 end) {
            transform.position = start;
            hit = end;
        }
    }
}
