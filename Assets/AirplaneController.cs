using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneController : MonoBehaviour {

    public bool stabilize;

    public Transform airplaneRoot;
    public List<AerodynamicWing> wings;
    public List<VectoredEngine> engines;
    
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
        Vector3 wantedTorque = Vector3.right * pSign + Vector3.forward * ySign + Vector3.up * rSign;

        if (stabilize) {
            engines.ForEach(e => {
                
            });
            for (int i=0; i < wings.Count; i++) {
                AerodynamicWing w = wings[i];
                if (w != null && w.canRotate) {
                    Quaternion invRot = Quaternion.Inverse(transform.rotation);
                    Vector3 rRelativeToTransform = w.GlobalLiftCenter - (transform.position + transform.rotation * localCoM);
                    Vector3 predictedTorque = Vector3.Cross(rRelativeToTransform, w.GlobalLiftNormal);
                    Vector3 relativeTorque = invRot * predictedTorque;
                    float pitch = 10 * Vector3.Dot(relativeTorque, Vector3.right) * pSign;
                    float roll = 10 * Vector3.Dot(relativeTorque, Vector3.forward) * ySign;
                    float yaw = 10 * Vector3.Dot(relativeTorque, Vector3.up) * rSign;
                    float angle = (pitch + roll + yaw);
                    w.flapAngle = angle;
                    Debug.DrawLine(transform.position, transform.position + predictedTorque, debugColors[i]);
                }
            }

            Debug.DrawLine(transform.position, transform.position + rb.velocity); 
        }

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
