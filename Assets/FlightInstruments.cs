using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlightInstruments : MonoBehaviour {

    public Text airspeed, altitude, vsi, groundspeed;

    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update () {
        Vector3 v = rb.velocity;
        float vs = Vector3.Dot(v, Vector3.up);
        Vector3 gs = v - vs * v;
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit);
        float alt = hit.distance;
        airspeed.text = string.Format("{}m/s", v.magnitude);
        vsi.text = string.Format("{}m/s", vs);
        groundspeed.text = string.Format("{}m/s", gs.magnitude);
        altitude.text = string.Format("{}m", alt);
    }
}
