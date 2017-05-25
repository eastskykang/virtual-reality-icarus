using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

   // public GameObject explosionPlayer;
   // public GameObject explosionCoin;

	//public AudioClip soundBird;
	//public AudioClip soundCoin;
	//public float volumeBird;
	public AudioClip[] Clips;
	private AudioSource audioSource;

	/*
	public AudioClip soundCoin;
	public float volumeCoin;
	AudioSource audioCoin;
    */
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
			//audioCoin.PlayOneShot (soundCoin, volumeCoin);
            Destroy(other.gameObject);
			gameController.TakeCoin (voice);
        }

		if (other.tag == "Bird") // Play Audio and decrease Player's health bar (take Damage)
        {
			audioSource.clip = Clips [0];
			audioSource.Play ();
			//audioSources [0].Play ();
			//audio[0].clip .PlayOneShot (soundBird);
			//audioBird.PlayOneShot (soundBird, volumeBird);
            Destroy(other.gameObject);
			gameController.TakeDamage (birdDamage);
        }

		if (other.tag == "Terrain")
		{
			gameController.TakeDamage (groundDamage); 
		}
    }
}
