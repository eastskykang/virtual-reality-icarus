/// <summary>
/// Game controller.
/// - Text
/// 1. health bar
///  From bird's attack, player's health decreases.
/// 2. Voice bar
///  After taking many coins, player can use his voice to ret rid of birds.
/// 
/// - Coin and Special function
/// Player can use "Voice bar" by making his loud voice to make energy wave which can get rid of birds around him.
/// And this continues until when "Voice bar" reaches 0.
/// 
/// - Interaction
/// 1. With terrain
///  Game Over
/// 2. With Coin
///  Increase "Voice bar" and he can make energy wave.
/// 3. With Bird
///  Decrease "Heath bar" and when it becomes 0, game over
/// 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text restartText;
    public Text gameoverText;
    public Text manualText;
	public Text timerText;
	private float startTime;

	public Image currentHealthBar;
	public Text ratioTextH;
	public Image currentVoiceBar;
	public Text ratioTextV;

	private float health = 200;
	private float maxHealth = 200;
	private float voice = 0;
	private float maxVoice = 100;

    public Camera[] cameras;
    private int currentCameraIndex;

    public GameObject coin;
    public GameObject bird;
	public GameObject cannon;

    public Transform playerLocation;

    public float coinArea = 100.0f;
    public float birdArea = 300.0f;
    public float coinGenTime = 1.0f;
    public float birdGenTime = 3.0f;

	private AudioSource audioSource;

    private bool game = true;

    private void Start()
    {
		audioSource = GetComponent<AudioSource> ();

		UpdateHealthBar ();
		UpdateVoiceBar ();

		manualText.text = "<Horzontal> Steering\n<Space> Flying\n<S> Special Function\n";
        restartText.text = "";
        gameoverText.text = "";
		startTime = Time.time;

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

		if (game){
			float t = Time.time - startTime;

			string minutes = ((int)t / 60).ToString ();
			string seconds = (t % 60).ToString ("f2");

			timerText.text = minutes + ":" + seconds;
		}
    }

	private void UpdateHealthBar()
	{
		float ratio = health / maxHealth;
		currentHealthBar.rectTransform.localScale = new Vector3 (ratio, 1, 1);
		ratioTextH.text = (ratio * 100).ToString("0") + '%';
	}

	public void TakeDamage(float damage)
	{
		health -= damage;

		if(health < 0){
			health = 0;
			Debug.Log ("Dead!");
			GameOver ();
		}

		UpdateHealthBar ();
	}


	private void UpdateVoiceBar()
	{
		float ratio = voice / maxVoice;
		currentVoiceBar.rectTransform.localScale = new Vector3 (ratio, 1, 1);
		ratioTextV.text = (ratio * 100).ToString("0") + '%';
	}

	public void TakeCoin(float coin)
	{
		if (voice <= maxVoice)
			voice += coin;
		
		if (voice < 0){
			voice = 0;
			Debug.Log ("Take coin first!");
			return;
		}

		if (coin < 0){
			Instantiate(cannon,  playerLocation.position, playerLocation.rotation);
		}

		UpdateVoiceBar ();
	}

	IEnumerator CoinGenerator()
	{
		while (game)
		{
			Vector3 coinPosition = playerLocation.position + playerLocation.transform.forward * coinArea;
			Quaternion coinRotation = Quaternion.identity;
			Instantiate(coin, coinPosition, coinRotation);
			yield return new WaitForSeconds(coinGenTime);
		}
	}

	IEnumerator BirdGenerator()
	{
		while (game)
		{
			float theta = Random.Range (20, 160.0f);
			theta *= (Mathf.PI / 180.0f);
			float length = Random.Range (100.0f, birdArea);

			Vector3 birdPosition = playerLocation.position
				+ playerLocation.transform.forward * length * Mathf.Sin (theta)
				+ playerLocation.transform.right * length * Mathf.Cos (theta);
			Quaternion birdRotation = Quaternion.identity;
			Instantiate(bird, birdPosition, birdRotation);
			yield return new WaitForSeconds(birdGenTime);
		}
	}

	private void GameOver()
	{
		audioSource.Stop ();

		//  Instantiate(explosionPlayer, other.transform.position, other.transform.rotation);
		GameObject player = GameObject.FindWithTag("Player");

		for (int i = 0; i < cameras.Length - 1; i++)
			cameras[i].gameObject.SetActive(false);

		cameras[cameras.Length - 1].gameObject.SetActive(true);
		cameras [cameras.Length - 1].gameObject.transform.position = player.transform.position + player.transform.up * 300;

		Destroy (player);

		manualText.text = "";
		restartText.text = "Press <R> to restart\nPress <ESC> to close";
		gameoverText.text = "GAME OVER";
		timerText.color = Color.red;
		game = false;
	}
}

