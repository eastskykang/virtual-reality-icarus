using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosionBird;
    public GameObject explosionPlayer;
    public GameObject explosionCoin;

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
        if (this.tag == "Player" && other.tag == "Coin")
        {
            gameController.AddScore(scoreValue);
            Instantiate(explosionCoin, other.transform.position, other.transform.rotation);
            Destroy(other.gameObject);
        }

        if (this.tag == "Bird" && other.tag == "Coin")
        {
            Instantiate(explosionBird, this.transform.position, this.transform.rotation);
            Destroy(gameObject);
            Destroy(other.gameObject);
        }

        if (this.tag == "Bird" && other.tag == "Player")
        {
            Instantiate(explosionBird, this.transform.position, this.transform.rotation);
            Instantiate(explosionPlayer, other.transform.position, other.transform.rotation);
            Destroy(gameObject);
            Destroy(other.gameObject);
            gameController.GameOver();
        }
    }
}
