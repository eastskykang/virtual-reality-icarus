using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

  //  public GameObject explosionBird;
   // public GameObject explosionPlayer;
  //  public GameObject explosionCoin;

	private float birdDamage = 5.0f;
	private float groundDamage = 500.0f;
	private float voice = 5.0f;

    public int scoreValue;
    private GameController gameController;

    private void Start()
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
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin")
        {
            //gameController.AddScore(scoreValue);
           // Instantiate(explosionCoin, other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
			gameController.TakeCoin (voice);
        }

		if (other.tag == "Bird")
        {
           // Instantiate(explosionBird, this.transform.position, this.transform.rotation);
            Destroy(other.gameObject);
			gameController.TakeDamage (birdDamage);
        }

		if (other.tag == "Terrain")
		{
			gameController.TakeDamage (groundDamage); 
		}
    }
}
