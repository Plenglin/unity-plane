using DroneCombat.Aerodynamics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DroneCombat.UI {

    public class PlayerController : MonoBehaviour {

        public AirplaneController airplane;
        public Camera fpCamera, tpCamera;
        public float enginePower = 1.0f;
        public float engineIncreaseRate = 2f;
        public bool firstPerson = true;

        // Use this for initialization
        private void Start() {

        }

        // Update is called once per frame
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                firstPerson = !firstPerson;
            }
            if (Input.GetKeyDown(KeyCode.T)) {
                airplane.stabilize = !airplane.stabilize;
            }

            if (Input.GetKey(KeyCode.LeftShift)) {
                enginePower += Time.deltaTime * engineIncreaseRate;
            }
            if (Input.GetKey(KeyCode.LeftControl)) {
                enginePower -= Time.deltaTime * engineIncreaseRate;
            }

            enginePower = Mathf.Clamp(enginePower, 0, 1);
            airplane.enginePower = enginePower;
            fpCamera.enabled = firstPerson;
            tpCamera.enabled = !firstPerson;
        }
    }
}