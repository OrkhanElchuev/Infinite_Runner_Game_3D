using System.Collections;
using UnityEngine;

/// <summary>
/// Spawns random obstacles forever at a given interval, within a horizontal range.
/// Spawn rate can be increased by decreasing obstacleSpawnTime (clamped to a minimum).
/// </summary>

public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Obstacle prefabs to pick from randomly.")]
    [SerializeField] GameObject[] obstaclePrefabs;
    [Tooltip("Parent transform for spawned obstacles.")]
    [SerializeField] Transform obstacleParent;


    [Header("Spawn Settings")]
    [Tooltip("Seconds between spawns (lower = more frequent obstacles).")]
    [SerializeField] float obstacleSpawnTime = 1f;
    [Tooltip("Minimum allowed seconds between spawns.")]
    [SerializeField] float minObstacleSpawnTime = 0.2f;
    [Tooltip("Half-width of spawn area on X (spawns between -spawnWidth and +spawnWidth).")]
    [SerializeField] float spawnWidth = 4f;

    WaitForSeconds spawnWait;

    void Start()
    {
        spawnWait = new WaitForSeconds(obstacleSpawnTime);
        StartCoroutine(SpawnObstacleRoutine());
    }

    // Decreases the spawn interval, increasing obstacle frequency.
    public void DecreaseObstacleSpawnTime(float amount)
    {
        obstacleSpawnTime -= amount;

        if (obstacleSpawnTime <= minObstacleSpawnTime)
        {
            obstacleSpawnTime = minObstacleSpawnTime;
        }
    }

    // Spawn obstacles with a delay indefinetely.
    IEnumerator SpawnObstacleRoutine()
    {
        while (true)
        {
            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

            // Random X within [-spawnWidth, spawnWidth], Y/Z follow spawner's transform.
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), transform.position.y, transform.position.z);
            yield return spawnWait;

            // Spawn Obstacles.
            Instantiate(obstaclePrefab, spawnPosition, Random.rotation, obstacleParent);
        }
    }
}
