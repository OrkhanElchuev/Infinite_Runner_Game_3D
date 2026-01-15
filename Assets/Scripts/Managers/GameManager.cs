using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TMP_Text timeText;
    [SerializeField] GameObject gameOverText;
    
    [Header("Settings")]
    [SerializeField] float startTime = 5f;

    float timeLeft;

    void Start()
    {
        timeLeft = startTime;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        // F1 for 1 decimal place formatting
        timeText.text = timeLeft.ToString("F1");

        if (timeLeft <= 0f)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOverText.SetActive(true);
        // Create a Slow motion effect
        Time.timeScale = 0.1f;
    }
}
