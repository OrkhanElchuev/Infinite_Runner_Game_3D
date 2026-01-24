using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Owns the main game state:
/// - counts down a level timer and updates UI
/// - triggers game over state when time reaches 0
/// - shows "Game Over" UI, slows time, and then enables restart UI
/// </summary>

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Player controller to disable when the player loses.")]
    [SerializeField] PlayerController playerController;
    [Tooltip("UI text that displays remaining time.")]
    [SerializeField] TMP_Text timeText;
    [Tooltip("UI element shown immediately on game over.")]
    [SerializeField] GameObject gameOverText;
    [Tooltip("UI element shown after a short delay to allow restart.")]
    [SerializeField] GameObject restartButton;
    
    [Header("Level Settings")]
    [Tooltip("How long (seconds) the player has at the start of the level.")]
    [SerializeField] float totalGameDuration = 5f;
    [Tooltip("How long (seconds, real-time) the game over text stays before showing restart.")]
    [SerializeField] float gameOverTextDuration = 2f;

    float timeLeft;
    bool gameOver = false;

    WaitForSecondsRealtime gameOverWait;

    // Game over flag for other systems.
    public bool GameOver => gameOver;

    void Start()
    {
        timeLeft = totalGameDuration;
        gameOverWait = new WaitForSecondsRealtime(gameOverTextDuration);

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
        yield return gameOverWait;

        // Hide "Game Over" and enable restart UI.
        if (gameOverText != null) gameOverText.SetActive(false);
        if (restartButton != null) restartButton.SetActive(true);
    }

    void DecreaseTime()
    {
        // Once game over is triggered, timer should stop ticking down.
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
        if (restartButton != null) restartButton.SetActive(false);
        if (gameOverText != null) gameOverText.SetActive(false);
    }

    void PlayerGameOver()
    {
        gameOver = true;

        // Disable player movement so input stops immediately.
        if (playerController != null) playerController.enabled = false;

        // Show game over UI (restart shows after delay).
        if (gameOverText != null) gameOverText.SetActive(true);
        if (restartButton != null) restartButton.SetActive(false);

        // Create a Slow motion effect
        Time.timeScale = 0.1f;

        StartCoroutine(ShowRestartAfterDelay());
    }
}
