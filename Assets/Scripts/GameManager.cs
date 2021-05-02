using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemies;
    public float enemySpawnTime = 5.0f;
    public float startDelay = 1.0f;
    public bool gameOn = false;
    public bool gameOver = false;
    public int wave = 0;
    public TMP_Text gameOverText;
    public TMP_Text waveText;
    public TMP_Text healthText;
    public Image elementSprite;
    public Sprite[] elementSprites;
    public int score = 0;
    public int clicks;
    public float accuracy;
    public TMP_Text scoreText;
    public Transform cameraPosition;
    public GameObject titleScreen;
    public GameObject gameUI;
    public GameObject endScreen;
    public TMP_Text finalWaveText;
    public TMP_Text finalAccuracyText;
    public TMP_Text finalScoreText;

    private float xRange = 15f;
    private float zRange = 9f;
    private float ySpawn = 0.5f;
    private float countdown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        titleScreen.SetActive(true);
        gameUI.SetActive(false);
        endScreen.SetActive(false);
        clicks = 0;
        accuracy = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOn)
        {
            if (countdown <= 0)
            {
                SpawnEnemy();
                countdown = enemySpawnTime;
            }
            countdown -= Time.deltaTime;
        }
    }

    void SpawnEnemy()
    {
        if (!gameOver)
        {
            wave++;
            waveText.text = "WAVE: " + wave;

            //every 5 waves, reduce spawn time, after 25, keep halving the spawn time
            if (wave < 5)
            {
                enemySpawnTime = 5f;
            }
            else if (wave < 10)
            {
                enemySpawnTime = 4f;
            }
            else if (wave < 15)
            {
                enemySpawnTime = 3f;
            }
            else if (wave < 20)
            {
                enemySpawnTime = 2f;
            }
            else if (wave < 25)
            {
                enemySpawnTime = 1f;
            }
            else
            {
                enemySpawnTime *= 0.99f;
            }

            float randomX = Random.Range(-xRange, xRange);
            float randomZ = Random.Range(-zRange, zRange);

            int randomIndex1 = Random.Range(0, enemies.Length);
            int randomIndex2 = Random.Range(0, enemies.Length);
            int randomIndex3 = Random.Range(0, enemies.Length);
            int randomIndex4 = Random.Range(0, enemies.Length);

            Vector3 spawnPos1 = new Vector3(xRange, ySpawn, randomZ);
            Vector3 spawnPos2 = new Vector3(-xRange, ySpawn, randomZ);
            Vector3 spawnPos3 = new Vector3(randomX, ySpawn, zRange);
            Vector3 spawnPos4 = new Vector3(randomX, ySpawn, -zRange);

            Instantiate(enemies[randomIndex1], spawnPos1, enemies[randomIndex1].gameObject.transform.rotation);
            Instantiate(enemies[randomIndex2], spawnPos2, enemies[randomIndex2].gameObject.transform.rotation);
            Instantiate(enemies[randomIndex3], spawnPos3, enemies[randomIndex3].gameObject.transform.rotation);
            Instantiate(enemies[randomIndex4], spawnPos4, enemies[randomIndex4].gameObject.transform.rotation);
        }
    }

    public void StartGame()
    {
        gameOver = false;
        gameOn = true;
        titleScreen.SetActive(false);
        gameUI.SetActive(true);
    }

    public void EndGame()
    {
        gameOver = true;
        gameOn = false;
        accuracy = (float)Mathf.Round(accuracy * 100f) / 100f;
        if (wave >= 50)
        {
            wave = 99;
            gameOverText.text = "YOU WIN";
        }
        finalWaveText.text = "WAVES: " + wave;
        finalAccuracyText.text = "ACCURACY: " + accuracy + "%";
        finalScoreText.text = "FINAL SCORE: " + score;
        endScreen.SetActive(true);
        gameUI.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateHealth(int health)
    {
        healthText.text = "HP: " + health;
    }

    public void UpdateElement(int element)
    {
        elementSprite.sprite = elementSprites[element];
    }

    public void IncrementScore()
    {
        score++;
        scoreText.text = "SCORE: " + score;
    }

    public void IncrementClicks()
    {
        clicks++;
        accuracy = (float)score / (float)clicks * 100f;
    }
}
