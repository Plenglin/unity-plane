using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneController : MonoBehaviour {

    public bool stabilize;

    public Transform airplaneRoot;
    public List<AerodynamicWing> wings;
    public List<VectoredEngine> engines;
    public Vector3 forward;

    private Quaternion forwardNormalizingRotation;    
    private Vector3 localCoM;
    private Rigidbody rb;
    private Color[] debugColors = {
        Color.black,
        Color.blue,
        Color.red,
        Color.cyan,
        Color.green,
        Color.grey,
        Color.magenta,
        Color.yellow
    };

    // Use this for initialization
    void Start () {
        UpdateCenterOfMass();
        rb = GetComponent<Rigidbody>();
        /*wings.ForEach(w => {
            w.rb = this.GetComponent<Rigidbody>();
        });*/
        forwardNormalizingRotation = Quaternion.FromToRotation(forward, Vector3.forward);
    }
	
	void FixedUpdate() {

        float ySign = 0;
        float pSign = 0;
        float rSign = 0;
        if (Input.GetKey(KeyCode.S)) {
            pSign = 1;
        }
        if (Input.GetKey(KeyCode.W)) {
            pSign = -1;
        }
        if (Input.GetKey(KeyCode.A)) {
            ySign = 1;
        }
        if (Input.GetKey(KeyCode.D)) {
            ySign = -1;
        }
        if (Input.GetKey(KeyCode.Q)) {
            rSign = -1;
        }
        if (Input.GetKey(KeyCode.E)) {
            rSign = 1;
        }
        Vector3 wantedTorque = forwardNormalizingRotation * (Vector3.right * pSign + Vector3.forward * ySign + Vector3.up * rSign).normalized;
        Vector3 globalWantedTorque = transform.rotation * wantedTorque;
        Vector3 globalCoM = transform.position + airplaneRoot.rotation * localCoM;

        engines.ForEach(e => {
                
        });
        if (wantedTorque.magnitude > 0.2) {
            for (int i = 0; i < wings.Count; i++) {
                AerodynamicWing w = wings[i];
                if (w != null && w.canRotate) {
                    w.flapAngle = pSign * w.pitchInfluence + rSign * w.rollInfluence + ySign * w.yawInfluence;
                }
            }
        } else {
            for (int i = 0; i < wings.Count; i++) {
                AerodynamicWing w = wings[i];
                if (w != null && w.canRotate) {
                    w.flapAngle = 0;
                }
            }
        }

        Debug.DrawLine(globalCoM, globalCoM + globalWantedTorque, Color.red);         

    }

    public void UpdateCenterOfMass() {
        Vector3 newCoM = new Vector3(0, 0, 0);
        float totalMass = 0;
        foreach (Transform child in Utils.AllChildrenOf(airplaneRoot)) {
            Rigidbody crb = child.GetComponent<Rigidbody>();
            if (crb != null) {
                newCoM += crb.mass * crb.worldCenterOfMass;
                totalMass += crb.mass;
            }
        }
        localCoM = newCoM / totalMass - transform.position;
    }

}
