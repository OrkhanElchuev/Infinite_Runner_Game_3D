using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerController playerController;
    [SerializeField] TMP_Text timeText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject restartButton;
    
    [Header("Settings")]
    [SerializeField] float startTime = 5f;
    [SerializeField] float gameOverTextDuration = 2f;

    float timeLeft;
    bool gameOver = false;

    // Add a property
    public bool GameOver => gameOver;

    void Start()
    {
        timeLeft = startTime;
        DeactivateText();
    }

    void Update()
    {
        DecreaseTime();
    }

    public void IncreaseTime(float amount)
    {
        timeLeft += amount;
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator ShowRestartAfterDelay()
    {
        yield return new WaitForSecondsRealtime(gameOverTextDuration);

        gameOverText.SetActive(false);
        restartButton.SetActive(true);
    }

    void DecreaseTime()
    {
        if (gameOver) return;

        timeLeft -= Time.deltaTime;
        // F1 for 1 decimal place formatting
        timeText.text = timeLeft.ToString("F1");

        if (timeLeft <= 0f)
        {
            PlayerGameOver();
        }
    }

    void DeactivateText()
    {
        restartButton.SetActive(false);
        gameOverText.SetActive(false);
    }

    void PlayerGameOver()
    {
        gameOver = true;
        playerController.enabled = false;

        gameOverText.SetActive(true);
        restartButton.SetActive(false);

        // Create a Slow motion effect
        Time.timeScale = 0.1f;

        StartCoroutine(ShowRestartAfterDelay());
    }
}
