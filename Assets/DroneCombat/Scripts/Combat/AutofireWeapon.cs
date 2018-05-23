using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DroneCombat.Combat {
    public class AutofireWeapon : MonoBehaviour {

        public float dps;

        private void Start() {

        }
        
        private void FixedUpdate() {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit)) {
                Health h = hit.collider.gameObject.GetComponent<Health>();
                if (h != null) {
                    h.health -= dps * Time.deltaTime;
                }
            }
        }

    }
}