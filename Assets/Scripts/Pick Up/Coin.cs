using UnityEngine;

/// <summary>
/// Coin pickup that increases the player's score.
/// Inherits pickup behavior (rotate + trigger + destroy) from PickUp.
/// </summary>

public class Coin : PickUp
{
    [Tooltip("How many points this coin is worth.")]
    [SerializeField] int scoreAmount = 100;

    // Assigned by Chunk.Init() when spawning coins.
    ScoreManager scoreManager;
    
    // Initializes the coin with required references.
    // Called by the Chunk that spawns it.
    public void Init(ScoreManager scoreManager) 
    {
        this.scoreManager = scoreManager;
    }

    protected override void OnPickup()
    {
        if (scoreManager == null) return;
        
        scoreManager.IncreaseScore(scoreAmount);
    }
}
