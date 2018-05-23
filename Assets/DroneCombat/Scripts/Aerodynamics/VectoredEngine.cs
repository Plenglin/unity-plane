using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DroneCombat.Aerodynamics {
    public class VectoredEngine : MonoBehaviour {

        public Rigidbody fuselage;
        public Vector3 maxForce;
        public float power;

        public float maxPhi;

        public float theta;
        public float phi;

        private ParticleSystem ps;

        private void Start() {
            ps = GetComponent<ParticleSystem>();
        }

        private void FixedUpdate() {
            Quaternion forceRot = Quaternion.Euler(0, 90 - phi, theta);
            float realPower = Mathf.Clamp(power, -1, 1);
            fuselage.AddForce(transform.rotation * maxForce * realPower);
            if (ps != null) {
                ps.Emit((int)(5 * realPower));
            }
        }

    }
}