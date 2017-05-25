using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMoving : MonoBehaviour {

    public float birdSpeed = 10.0f;
    private Rigidbody bikeRigidbody;
	//private GameController gameController;

    void Start () {
        bikeRigidbody = GetComponent<Rigidbody>();
	}

    private void Update()
    {
		GameObject gamePlayer = GameObject.FindWithTag("Player");
		if (gamePlayer != null)
		{
			bikeRigidbody.transform.LookAt(gamePlayer.transform);
		}

		if (gamePlayer == null)
		{
			Destroy (this.gameObject);
		}

        bikeRigidbody.velocity = transform.forward * birdSpeed;
    }

}
