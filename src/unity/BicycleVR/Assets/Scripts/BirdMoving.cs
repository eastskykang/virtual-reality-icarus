using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMoving : MonoBehaviour {

    public float birdSpeed = 7.0f;
    public Transform player;

	void Start () {
        Rigidbody rigidbody = (Rigidbody)GetComponent(typeof(Rigidbody));
        rigidbody.transform.LookAt(player);
        rigidbody.velocity = transform.forward * birdSpeed;
	}
	
}
