using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContact : MonoBehaviour {

	public AudioClip[] Clips;
	private AudioSource audioSource;

	private float birdDamage = 5.0f;
	private float groundDamage = 500.0f;
	private float voice = 5.0f;

    private GameController gameController;

    private void Start()
    {
		audioSource = GetComponent<AudioSource> ();
		
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
        if (other.tag == "Coin") // Play Audio and increase Player's voice bar
        {
			audioSource.clip = Clips [1];
			audioSource.Play ();
            Destroy(other.gameObject);
			gameController.TakeCoin (voice);
        }

		if (other.tag == "Bird") // Play Audio and decrease Player's health bar (take Damage)
        {
			audioSource.clip = Clips [0];
			audioSource.Play ();
            Destroy(other.gameObject);
			gameController.TakeDamage (birdDamage);
        }

		if (other.tag == "Terrain")
		{
			gameController.TakeDamage (groundDamage); 
		}
    }
}
