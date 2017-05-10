using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovingControl : MonoBehaviour
{
    public float maxAngle = 15f;
    public float maxTorque = 50f;

    public WheelCollider[] wheelColliders = new WheelCollider[4];

    private Rigidbody bikeRigidbody;

    // Use this for initialization
    void Start () {
        bikeRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        float steer = Input.GetAxis("Horizontal");
        float acclerate = Input.GetAxis("Vertical");
        float angle = steer * maxAngle;
        float torque = acclerate * maxTorque;

        for (int i = 0; i < 2; i++)
        {
            wheelColliders[i].steerAngle = angle;
        }

        for (int i = 0; i < 4; i++)
        {
            wheelColliders[i].motorTorque = torque;
        }
    }
}
