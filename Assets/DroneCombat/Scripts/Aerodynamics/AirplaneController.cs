using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DroneCombat.Aerodynamics {

    public class AirplaneController : MonoBehaviour {

        public bool stabilize;

        public Transform airplaneRoot;
        public Vector3 forward;
        public float yawResponse, pitchResponse, rollResponse;
        public float enginePower;

        private List<Wing> wings = new List<Wing>();
        private List<VectoredEngine> engines = new List<VectoredEngine>();
        private Quaternion forwardNormalizingRotation;
        private Vector3 localCoM;
        private Rigidbody rb;

        // Use this for initialization
        void Start() {
            UpdateChildren();
            rb = GetComponent<Rigidbody>();
            /*wings.ForEach(w => {
                w.rb = this.GetComponent<Rigidbody>();
            });*/
            forwardNormalizingRotation = Quaternion.FromToRotation(forward, Vector3.forward);
        }

        void FixedUpdate() {

            bool shouldStabilize = true;
            float yaw = 0;
            float pitch = 0;
            float roll = 0;

            // User control
            if (Input.GetKey(KeyCode.S)) {
                pitch += 1;
                shouldStabilize = false;
            }
            if (Input.GetKey(KeyCode.W)) {
                pitch += -1;
                shouldStabilize = false;
            }
            if (Input.GetKey(KeyCode.A)) {
                yaw += 1;
                shouldStabilize = false;
            }
            if (Input.GetKey(KeyCode.D)) {
                yaw += -1;
                shouldStabilize = false;
            }
            if (Input.GetKey(KeyCode.Q)) {
                roll += -1;
                shouldStabilize = false;
            }
            if (Input.GetKey(KeyCode.E)) {
                roll += 1;
                shouldStabilize = false;
            }

            // Stabilization control
            if (stabilize && shouldStabilize) {
                Vector3 w = Quaternion.Inverse(rb.rotation) * rb.angularVelocity;
                yaw += yawResponse * Vector3.Dot(w, Vector3.up);
                pitch += pitchResponse * Vector3.Dot(w, Vector3.right);
                roll += rollResponse * Vector3.Dot(w, Vector3.forward);
            }

            for (int i = 0; i < wings.Count; i++) {
                Wing w = wings[i];
                if (w != null && w.canRotate) {
                    w.flapAngle = pitch * w.pitchInfluence + roll * w.rollInfluence + yaw * w.yawInfluence;
                }
            }

            // Engines
            engines.ForEach(e => {
                e.power = enginePower;
            });

        }

        public void UpdateChildren() {
            wings.Clear();
            Vector3 newCoM = new Vector3(0, 0, 0);
            float totalMass = 0;
            foreach (Transform child in Utils.AllChildrenOf(airplaneRoot)) {
                Rigidbody crb = child.GetComponent<Rigidbody>();
                if (crb != null) {
                    newCoM += crb.mass * crb.worldCenterOfMass;
                    totalMass += crb.mass;
                }
                Wing cw = child.GetComponent<Wing>();
                if (cw != null) {
                    wings.Add(cw);
                }
                VectoredEngine ce = child.GetComponent<VectoredEngine>();
                if (ce != null) {
                    engines.Add(ce);
                }
            }
            localCoM = newCoM / totalMass - transform.position;
        }

    }
}
