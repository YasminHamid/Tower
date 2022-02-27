using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private CubeSpawner[] spawners;
    private int spawnerIndex;
    private CubeSpawner currentSpawner;
    private bool isGameover = false;

    //text
    public GameObject gameOverTextObj;

    //sounds
    private AudioSource gameoverSound;
    private AudioSource backGroundSound;

    //buttons
    public GameObject retryButton;

    private void Awake()
    {
        gameOverTextObj.SetActive(false);
        retryButton.SetActive(false);

        spawners = FindObjectsOfType<CubeSpawner>();
        AudioSource[] sounds = GetComponents<AudioSource>();
        gameoverSound = sounds[0];
        backGroundSound = sounds[1];
    }

    void Update()
    {
        if (!isGameover && Input.GetButtonDown("Fire1"))
        {
            if (MovingCube.currentCube != null)
                MovingCube.currentCube.stop();

            spawnerIndex = UnityEngine.Random.Range(0, 4);
            currentSpawner = spawners[spawnerIndex];

            currentSpawner.spawnCube();
            Camera.main.transform.Translate(0, 0.1f, 0);
        }
        
    }

    public void GameOver()
    {
        gameOverTextObj.SetActive(true);
        Time.timeScale = 0;
        backGroundSound.Stop();
        gameoverSound.Play();
        isGameover = true;
        retryButton.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        isGameover = false;
        Time.timeScale = 1f;
    }

}
