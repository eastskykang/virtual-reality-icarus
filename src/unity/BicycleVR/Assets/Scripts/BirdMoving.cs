using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMoving : MonoBehaviour {

    public float birdSpeed = 10.0f;
    private Rigidbody rigidbody;

    void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}

    private void Update()
    {
		GameObject gamePlayer = GameObject.FindWithTag("Player");
		if (gamePlayer != null)
		{
			rigidbody.transform.LookAt(gamePlayer.transform);
		}

		if (gamePlayer == null)
		{
			Destroy (this.gameObject);
		}

        rigidbody.velocity = transform.forward * birdSpeed;
    }
}
