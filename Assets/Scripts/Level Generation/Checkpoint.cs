using UnityEngine;

public class Checkpoint : MonoBehaviour
{   
    [SerializeField] float checkpointTimeExtension = 5f;

    const string playerString = "Player";

    GameManager gameManager;

    void Start()
    {
        // Instantiated every 8 chunk, not a performance bottleneck
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerString))
        {
            gameManager.IncreaseTime(checkpointTimeExtension);
        }
    }
}
