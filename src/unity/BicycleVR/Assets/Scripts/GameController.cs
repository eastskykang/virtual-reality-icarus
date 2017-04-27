using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text scoreText;
    private int score;
    public Text restartText;
    public Text gameoverText;
    public Text manualText;

    public Camera[] cameras;
    private int currentCameraIndex;

    public GameObject coin;
    public GameObject bird;

    public Transform playerLocation;

    public float coinArea = 20.0f;
    public float birdArea = 100.0f;
    public float coinTime = 5.0f;
    public float birdTime = 10.0f;

    private bool game = true;

    private void Start()
    {
        manualText.text = "<Vertical> Power\n<Horzontal> Steering\n<Space> Flying\n<C> Change View";
        restartText.text = "";
        gameoverText.text = "";
        score = 0;
        Updatescore();

        currentCameraIndex = 0;

        if (cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);

            for (int i = 1; i < cameras.Length; i++)
                cameras[i].gameObject.SetActive(false);
        }

        StartCoroutine(CoinGenerator());
        StartCoroutine(BirdGenerator());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentCameraIndex++;

            if (currentCameraIndex < cameras.Length - 1)
            {
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                cameras[currentCameraIndex].gameObject.SetActive(true);
            }
            else
            {
                currentCameraIndex = 0;
                cameras[cameras.Length - 2].gameObject.SetActive(false);
                cameras[currentCameraIndex].gameObject.SetActive(true);
            }

        }

        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(Application.loadedLevel);

        if (Input.GetKeyDown(KeyCode.Escape))
            //Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
    }

    IEnumerator CoinGenerator()
    {
        while (game)
        {
            Vector3 coinPosition = new Vector3(Random.Range(playerLocation.position.x, playerLocation.position.x + coinArea),
                                               playerLocation.position.y,
                                               Random.Range(playerLocation.position.z, playerLocation.position.z + coinArea));
            Quaternion coinRotation = Quaternion.identity;
            Instantiate(coin, coinPosition, coinRotation);
            yield return new WaitForSeconds(coinTime);
        }
    }

    IEnumerator BirdGenerator()
    {
        while (game)
        {
            Vector3 birdPosition = new Vector3(Random.Range(playerLocation.position.x, playerLocation.position.x + birdArea),
                                               playerLocation.position.y,
                                               Random.Range(playerLocation.position.z, playerLocation.position.z + birdArea));
            Quaternion birdRotation = Quaternion.identity;
            Instantiate(bird, birdPosition, birdRotation);
            yield return new WaitForSeconds(birdTime);
        }
    }

    public void GameOver()
    {
        for (int i = 0; i < cameras.Length - 1; i++)
            cameras[i].gameObject.SetActive(false);

        cameras[cameras.Length - 1].gameObject.SetActive(true);

        restartText.text = "Press <R> to restart\nPress <ESC> to close";
        gameoverText.text = "GAME OVER";
        game = false;
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        Updatescore();
    }

    private void Updatescore()
    {
        scoreText.text = "Score: " + score;
    }
}