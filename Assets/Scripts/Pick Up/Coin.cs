using UnityEngine;

public class Coin : PickUp
{
    [SerializeField] int scoreAmount = 100;
    ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    protected override void OnPickup()
    {
        scoreManager.IncreaseScore(100);
    }
}
