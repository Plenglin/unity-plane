using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneController : MonoBehaviour {

    public bool stabilize;

    public Transform airplaneRoot;
    public List<AerodynamicWing> wings;
    public List<VectoredEngine> engines;
    public Vector3 forward;
    public float yawResponse, pitchResponse, rollResponse;

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

        Vector3 wantedTorque = forwardNormalizingRotation * (Vector3.right * pitch + Vector3.forward * yaw + Vector3.up * roll).normalized;
        Vector3 globalWantedTorque = transform.rotation * wantedTorque;
        Vector3 globalCoM = transform.position + airplaneRoot.rotation * localCoM;

        engines.ForEach(e => {
                
        });
        if (wantedTorque.magnitude > 0.2) {
            for (int i = 0; i < wings.Count; i++) {
                AerodynamicWing w = wings[i];
                if (w != null && w.canRotate) {
                    w.flapAngle = pitch * w.pitchInfluence + roll * w.rollInfluence + yaw * w.yawInfluence;
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
