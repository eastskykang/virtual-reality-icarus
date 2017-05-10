using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float forwardSpeed = 20.0f;
    public float rotationSpeed = 2.0f; 

	private float flyingSpeed = 200.0f;
    private Rigidbody bikeRigidbody;

    void Start ()
	{
        bikeRigidbody = GetComponent<Rigidbody>();
    }

    void Update ()
	{
        transform.position += transform.forward * Time.deltaTime * forwardSpeed;
        transform.Rotate(0.0f , Input.GetAxis("Horizontal") * rotationSpeed, 0.0f);

		RaycastHit hit;
		Ray landingRay = new Ray (transform.position, Vector3.down);
		Physics.Raycast(landingRay, out hit);

		if (Input.GetKey(KeyCode.Space))
		{
			bikeRigidbody.AddForce(transform.up * (flyingSpeed - 2*hit.distance));
		}
    }
}
