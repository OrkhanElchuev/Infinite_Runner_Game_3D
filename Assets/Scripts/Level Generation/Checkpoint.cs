using UnityEngine;

public class Checkpoint : MonoBehaviour
{   
    [SerializeField] float checkpointTimeExtension = 5f;
    [SerializeField] float obstacleDecreaseAmount = 0.2f;

    const string playerString = "Player";

    GameManager gameManager;
    ObstacleSpawner obstacleSpawner;

    void Start()
    {
        // Instantiated every 8 chunk, not a performance bottleneck
        gameManager = FindFirstObjectByType<GameManager>();
        obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerString))
        {
            gameManager.IncreaseTime(checkpointTimeExtension);
            obstacleSpawner.DecreaseObstacleSpawnTime(obstacleDecreaseAmount);
        }
    }
}
