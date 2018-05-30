using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DroneCombat.Combat {
    public class HitscanWeapon : MonoBehaviour {

        public float damage;
        public float delay;
        public float range;
        public GameObject tracerPrefab;

        public float momentum;
        public float penetration = 1;
        public float reflection = 1;

        private float lastFired;

        private void Start() {
            lastFired = 0;
        }
        
        private void FixedUpdate() {
            if (Time.time > lastFired) {
                Fire();
            }
        }

        public void Fire() {
            RaycastHit hit;
            Vector3 tracerEnd;
            if (Physics.Raycast(transform.position, transform.forward, out hit, range)) {
                GameObject go = hit.collider.gameObject;
                Health hp = go.GetComponent<Health>();
                Rigidbody rb = go.GetComponent<Rigidbody>();
                if (hp != null) {
                    hp.Damage(damage);
                }
                if (rb != null) {
                    // Calculate the force exerted on the bullet, then on the cube by Newton's 3rd Law
                    Vector3 reflectionForce = 2 * Vector3.Dot(transform.forward, hit.normal) * hit.normal - transform.forward;
                    Vector3 absorptionForce = transform.forward;
                    rb.AddForceAtPosition(momentum * (reflection * reflectionForce + penetration * absorptionForce), hit.point, ForceMode.Impulse);
                }
                tracerEnd = hit.point;
            } else {
                tracerEnd = transform.position + transform.forward * range;
            }
            Instantiate(tracerPrefab).GetComponent<Tracer>().PlaceAt(transform.position, tracerEnd);
            lastFired = Time.time + delay;
        }

    }
}