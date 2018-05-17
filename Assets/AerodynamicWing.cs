using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerodynamicWing : MonoBehaviour {

    public Vector3 liftAxis, liftCenter, flapAxis;
    public float flapLimit;
    public float flapAngle;
    public bool canRotate;
    public GameObject render;
    public float drag;

    //private Vector3 prevPos;

    [NonSerialized]
    private Rigidbody rb;

    public Vector3 GlobalLiftCenter {
        get {
            return transform.position + liftCenter;
        }
    }
    public Vector3 GlobalLiftNormal {
        get {
            return transform.rotation * liftAxis;
        }
    }

    public Vector3 LocalLiftAtAngle(float a) {
        float angleClamped = Mathf.Clamp(a, -flapLimit, flapLimit);
        return Quaternion.AngleAxis(angleClamped, flapAxis) * liftAxis;
    }

    public Vector3 GlobalLiftAtAngle(float a) {
        return transform.rotation * LocalLiftAtAngle(a);
    }

    private void Start() {
        //prevPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        //Vector3 v = (transform.position - prevPos) / Time.fixedDeltaTime;

        // Transform the velocities to local space
        Vector3 v = Quaternion.Inverse(transform.rotation) * rb.velocity;
        Vector3 v2 = v * v.magnitude;

        // Perform mathy shit
        float liftCoeff = liftAxis.magnitude;
        Vector3 liftNorm = liftAxis / liftCoeff;
        float angleClamped = Mathf.Clamp(flapAngle, -flapLimit, flapLimit);
        Vector3 liftNormRot = Quaternion.AngleAxis(angleClamped, flapAxis) * liftNorm;

        Vector3 liftLocalFinal = -Vector3.Dot(v2, liftNormRot) * liftNormRot * liftCoeff;

        Vector3 dragLocalFinal = -v2 * this.drag;

        // Transform the vectors to world space
        Vector3 finalGlobalForce = transform.rotation * (dragLocalFinal + liftLocalFinal);
        rb.AddForceAtPosition(finalGlobalForce, GlobalLiftCenter);

        // Visual debug shit
        //Debug.DrawLine(GlobalLiftCenter, GlobalLiftCenter + transform.rotation * liftLocalFinal / 10);
        //Debug.DrawLine(GlobalLiftCenter, GlobalLiftCenter + GlobalLiftNormal);
        if (canRotate) {
            Quaternion flapRotation = Quaternion.AngleAxis(angleClamped, flapAxis);
            render.transform.rotation = transform.rotation * flapRotation;
        }
        //prevPos = transform.position;
	}
}
