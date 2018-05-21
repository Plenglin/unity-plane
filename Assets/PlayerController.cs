using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public AirplaneController airplane;
    public Camera fpCamera, tpCamera;
    private bool firstPerson = true;

	// Use this for initialization
	private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Tab)) {
            firstPerson = !firstPerson;
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            airplane.stabilize = !airplane.stabilize;
        }

        fpCamera.enabled = firstPerson;
        tpCamera.enabled = !firstPerson;
    }
}
