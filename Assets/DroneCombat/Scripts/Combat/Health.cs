using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Collections;

namespace DroneCombat.Combat {
    public class Health : MonoBehaviour {
        
        public float health;
        public float maxHealth;

        private void Start() {
            health = maxHealth;
        }

        private void Update() {

        }
    }
}