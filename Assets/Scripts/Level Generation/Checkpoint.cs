using UnityEngine;

/// <summary>
/// Checkpoint trigger that rewards the player by:
/// - extending the remaining game time
/// - slightly increasing difficulty by reducing obstacle spawn interval
/// Intended to be placed on checkpoint chunks and triggered by the Player.
/// </summary>

public class Checkpoint : MonoBehaviour
{   
    [Header("Checkpoint Rewards")]
    [Tooltip("How much time (seconds) is added when the player hits this checkpoint.")]
    [SerializeField] float checkpointTimeExtension = 5f;
    [Tooltip("How much to decrease obstacle spawn time (lower = more frequent obstacles).")]
    [SerializeField] float obstacleDecreaseAmount = 0.2f;

    const string PLAYER_TAG = "Player";

    GameManager gameManager;
    ObstacleSpawner obstacleSpawner;

    void Start()
    {
        // Instantiated infrequently (every N chunks).
        gameManager = FindFirstObjectByType<GameManager>();
        obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
    }

    void OnTriggerEnter(Collider other)
    {
        // CompareTag is faster and safer than other.tag == "Player".
        if (!other.CompareTag(PLAYER_TAG)) return;

        gameManager?.IncreaseTime(checkpointTimeExtension);
        obstacleSpawner?.DecreaseObstacleSpawnTime(obstacleDecreaseAmount); 
    }
}
