using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float forwardSpeed = 20.0f;
    public float rotationSpeed = 2.0f; 
	public float flyingPower = 200.0f;

    private Rigidbody bikeRigidbody;
	private GameController gameController;

    void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameControllerObject == null)
		{
			Debug.Log("Cannot find 'GameController' script");
		}

		bikeRigidbody = GetComponent<Rigidbody>();
    }

    void Update ()
	{
		//Forward
		transform.position += transform.forward * Time.deltaTime * forwardSpeed;

		//Rotation
        transform.Rotate(0.0f , Input.GetAxis("Horizontal") * rotationSpeed, 0.0f);

		//Flying
		RaycastHit hit;
		Ray landingRay = new Ray (transform.position, Vector3.down);
		Physics.Raycast(landingRay, out hit);

		if (Input.GetKey(KeyCode.Space))
			bikeRigidbody.AddForce(transform.up * flyingPower);
			//* (flyingPower - 2*hit.distance)

		//Special Function
		if (Input.GetKeyDown(KeyCode.S)){
			gameController.TakeCoin (-1.0f);
		}
    }
}
