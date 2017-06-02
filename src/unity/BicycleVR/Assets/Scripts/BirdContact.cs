using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdContact : MonoBehaviour {

	public GameObject birdExplosion;
	public GameObject birdGrowing;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Cannon")
		{
			Instantiate (birdExplosion, other.transform.position, other.transform.rotation);
			Destroy(other.gameObject);
			Destroy (this.gameObject);
		}

		if (other.tag == "Coin")
		{
			Instantiate (birdGrowing, other.transform.position, other.transform.rotation);
			Destroy(other.gameObject);
			this.gameObject.transform.localScale += new Vector3 (1, 1, 1);
			Debug.Log ("I'm Growing");
		}
	}
}
