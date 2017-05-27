using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonMoving : MonoBehaviour {
	
    public float speed = 100.0f;

	private Rigidbody rigidbody;

	void Start () {
		rigidbody = GetComponent<Rigidbody>();
	}

	void Update () {
		rigidbody.velocity = transform.forward * speed;
	}
}