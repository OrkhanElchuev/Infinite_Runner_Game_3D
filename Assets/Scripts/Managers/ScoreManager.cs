using TMPro;
using UnityEngine;

/// <summary>
/// Tracks the player's score and updates the score UI.
/// Does not increase score after the game is over.
/// </summary>

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("UI text displaying the current score.")]
    [SerializeField] TMP_Text scoreText;
    [Tooltip("GameManager used to check if the game is over.")]
    [SerializeField] GameManager gameManager;

    [Header("State")]
    [Tooltip("Current score value (read-only in Inspector).")]
    [SerializeField] private int score = 0;

    private void Start()
    {
        // Ensure the UI is correct at the beginning of the level.
        UpdateScoreUI();
    }

    /// Adds to the score and updates the UI.
    /// If the game is over, scoring is ignored.
    public void IncreaseScore(int amount)
    {
        // If the reference isn't wired allow scoring.
        if (gameManager != null && gameManager.GameOver) return;

        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText == null) return;
        scoreText.text = score.ToString();
    }
}
