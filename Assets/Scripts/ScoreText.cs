using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    private int score = 0;
    private TextMeshProUGUI text;
    private int highScore;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("highScore", highScore);
        text = GameObject.Find("ScoreText").GetComponent<TMPro.TextMeshProUGUI>();
        MovingCube.onCubeStop += MovingCube_OnCubeStop;
        MovingCube.onGameOver += MovingCube_OnGameOver;
    }

    private void OnDestroy()
    {
        MovingCube.onCubeStop -= MovingCube_OnCubeStop;
        MovingCube.onGameOver -= MovingCube_OnGameOver;
    }

    private void MovingCube_OnCubeStop()
    {
        score++;
        text.text = "Score: " + score;
    }

    private void MovingCube_OnGameOver()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
        }
        text.text = "Score: " + score + "\u000aHigh Score: " + highScore;
    }

}
