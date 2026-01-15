using System.Collections;
using UnityEngine;


public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject[] obstaclePrefabs;
    [SerializeField] Transform obstacleParent;


    [Header("Spawn Settings")]
    [SerializeField] float obstacleSpawnTime = 1f;
    [SerializeField] float spawnWidth = 4f;


    void Start()
    {
       StartCoroutine(SpawnObstacleRoutine());
    }

    // Spawn obstacles with a delay indefinetely
    IEnumerator SpawnObstacleRoutine()
    {
        while (true)
        {
            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), transform.position.y, transform.position.z);
            // Add a Spawn Delay
            yield return new WaitForSeconds(obstacleSpawnTime);
            // Spawn Obstacles
            Instantiate(obstaclePrefab, spawnPosition, Random.rotation, obstacleParent);
        }
    }
}
