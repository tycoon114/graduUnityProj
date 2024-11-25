using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    GameObject player;

    int score = 0;

    public bool isPlayerAlive = true;

    public Text scoreText;

    public static GameManager instance;

    private void Awake()
    {
        if (GameManager.instance == null)
        {
            GameManager.instance = this;
        }

    }
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        Invoke("StartGame", 3f);

	}
    void StartGame()
    {
        player.GetComponent<Player>().canShoot = true;
        SpawnManager.instance.isSpawn = true;
    }
    public void ResetGame()
    {
         ObjectManager.instance.ClearBullets();
        SpawnManager.instance.ClearEnemies();

        score = 0;
        scoreText.text = string.Empty;

        TextContrl.instance.Restart();
        Invoke("Retry Game", 3f);
       

    }
     void RetryGame()
        {
        StartGame();
        player.SetActive(true);

        }
    public void KillPlayer()
    {
        isPlayerAlive = false;
        SpawnManager.instance.isSpawn = false;
        TextContrl.instance.ShowGameOver();
    }

    public void AddScore(int enemyScore)// 점수가 올라가는 함수
    {
        score += enemyScore;
        scoreText.text = "Score:" + score;
    }


    void Update () {
		
	}
}
