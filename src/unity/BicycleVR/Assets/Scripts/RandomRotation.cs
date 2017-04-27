using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour {

    public float tumble = 5.0f;

    private void Start()
    {
        Rigidbody rigidbody = (Rigidbody)GetComponent(typeof(Rigidbody));
        rigidbody.angularVelocity = Random.insideUnitSphere * tumble;
    }
}
