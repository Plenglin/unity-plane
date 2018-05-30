using DroneCombat.Aerodynamics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DroneCombat.Combat {

    public class WingDamage : MonoBehaviour {

        private float initialBreakForce, initialLift;
        private Wing wing;
        private Joint joint;
        private Health health;

        // Use this for initialization
        void Start() {
            wing = GetComponent<Wing>();
            joint = GetComponent<Joint>();
            health = GetComponent<Health>();

            initialBreakForce = joint.breakForce;
            initialLift = wing.liftAxis.magnitude;
        }

        void OnDamage() {
            float hf = health.GetHealthFraction();
            wing.liftAxis.Normalize();
            wing.liftAxis *= hf * initialLift;
            joint.breakForce = hf * initialBreakForce;
        }
    }
}