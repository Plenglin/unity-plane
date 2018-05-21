using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlightInstruments : MonoBehaviour {

    public Text airspeed, altitude, vsi, groundspeed, elevation, sas;

    private AirplaneController airplane;
    private Rigidbody rb;

    private void Start() {
        airplane = GetComponent<AirplaneController>();
        rb = GetComponent<Rigidbody>();
    }

    void Update () {
        Vector3 v = rb.velocity * 10;
        float vs = Vector3.Dot(v, Vector3.up);
        Vector3 gs = v - vs * Vector3.up;
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit);
        float alt = hit.distance * 10;
        float elev = transform.position.y * 10;

        airspeed.text = string.Format("{0:0.0}m/s", v.magnitude);
        vsi.text = string.Format("{0:0.0}m/s", vs);
        groundspeed.text = string.Format("{0:0.0}m/s", gs.magnitude);
        altitude.text = string.Format("{0:0.0}m", alt);
        elevation.text = string.Format("{0:0.0}m", elev);
        sas.text = "SAS: " + (airplane.stabilize ? "ON" : "OFF");
    }
}
