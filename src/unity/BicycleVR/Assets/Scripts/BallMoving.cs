using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMoving : MonoBehaviour {
	
    public float speed = 100.0f;
	public GameObject explosionBird;

	private Rigidbody rigidbody;

	void Start () {
		rigidbody = GetComponent<Rigidbody>();
	}

	void Update () {
		rigidbody.velocity = transform.forward * speed;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Bird") {
			Instantiate (explosionBird, other.transform.position, other.transform.rotation);
			Destroy (other.gameObject);
			Destroy (this.gameObject);
		}
	}
}