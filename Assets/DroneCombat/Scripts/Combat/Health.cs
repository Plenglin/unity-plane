using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Collections;
using UnityEngine.Events;

namespace DroneCombat.Combat {
    public class Health : MonoBehaviour {

        public UnityEvent onDamage, onRepair;

        private float health;
        public float maxHealth;

        private void Start() {
            health = maxHealth;
        }

        public float GetHealth() {
            return health;
        }
        
        public float GetHealthFraction() {
            return health / maxHealth;
        }

        public void Damage(float hp) {
            onDamage.Invoke();
            health -= hp;
        }

        public void Repair(float hp) {
            onRepair.Invoke();
            health += hp;
        }
    }
}