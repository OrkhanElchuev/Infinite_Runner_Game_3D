using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A level "chunk" that spawns obstacles and collectibles on lanes.
/// Each chunk is responsible for populating itself when created.
/// </summary>

public class Chunk : MonoBehaviour
{   
    [Header("References")]
    [Tooltip("Obstacle prefab placed on lanes.")]
    [SerializeField] GameObject fencePrefab;
    [Tooltip("Collectible apple prefab (affects speed or similar via Apple script).")]
    [SerializeField] GameObject applePrefab;
    [Tooltip("Collectible coin prefab (increases score via Coin script).")]
    [SerializeField] GameObject coinPrefab;
    
    [Header("Settings")]
    [Tooltip("Chance [0..1] to spawn an apple on this chunk.")]
    [SerializeField] float appleSpawnChance = 0.2f;
    [Tooltip("Chance [0..1] to spawn a coin line on this chunk.")]
    [SerializeField] float coinSpawnChance = 0.5f;
    [Tooltip("Distance between coins along the Z axis.")]
    [SerializeField] float coinSeperationLength = 2f;
    
    [Header("Lanes")]
    [Tooltip("X positions for each lane index (0..lanes.Length-1).")]
    [SerializeField] float[] lanes = {-2.5f, 0f, 2.5f};

    LevelGenerator levelGenerator;
    ScoreManager scoreManager;
    
    // Tracks which lanes are still free so objects don't overlap.
    List<int> availableLanes = new List<int> {0, 1, 2};
    
    void Start()
    {
        SpawnFences();
        SpawnApple();
        SpawnCoins();
    }

    // Provides references needed by spawned items to communicate back to systems.
    public void Init(LevelGenerator levelGenerator, ScoreManager scoreManager)
    {
        this.levelGenerator = levelGenerator;
        this.scoreManager = scoreManager;
    }

    // Selects a lane index from the lanes that are still free.
    int SelectLane()
    {
        // Find and select an available lane without an already instantiated object.
        int randomLaneIndex = Random.Range(0, availableLanes.Count);
        int selectedLane = availableLanes[randomLaneIndex];
        // Remove it from the list to not instantiate another object on the same lane.
        availableLanes.RemoveAt(randomLaneIndex);
        return selectedLane;
    }

    void SpawnFences()
    {
        // Spawn between 0 and lanes.Length - 1 fences.
        int fencesToSpawn = Random.Range(0, lanes.Length);

        // Make sure there is always at least 1 available lane for a player
        for (int i = 0; i < fencesToSpawn; i++)
        {
            if (availableLanes.Count <= 0) break;
            
            int selectedLane = SelectLane();

            // Place fence aligned to the chunk's center Z and Y.
            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
            Instantiate(fencePrefab, spawnPosition, Quaternion.identity, this.transform);
        }
    }

    void SpawnApple()
    {
        // If Random.value is in [0..1), spawn if value <= chance.
        if (Random.value > appleSpawnChance || availableLanes.Count <= 0) return;

        int selectedLane = SelectLane();
        Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);

        // Instantiate and initialize apple with LevelGenerator Reference.
        Apple newApple = Instantiate(applePrefab, spawnPosition, Quaternion.identity, this.transform).GetComponent<Apple>();
        if (newApple != null)
        {
            newApple.Init(levelGenerator);
        }
    }

    void SpawnCoins()
    {
        // 50 % chance to spawn a Coin.
        if (Random.value > coinSpawnChance || availableLanes.Count <= 0) return;

        int selectedLane = SelectLane();

        // Coins form a short line leading into the chunk so the player can collect them while moving forward.
        int maxCoinsToSpawn = 6;
        int coinsToSpawn = Random.Range(0, maxCoinsToSpawn);
        // Start a bit ahead of the chunk center to distribute coins along Z.
        float topOfChunkZPos = transform.position.z + (coinSeperationLength * 2f);

        for (int i = 0; i < coinsToSpawn; i++)
        {
            float spawnPositionZ = topOfChunkZPos - (i * coinSeperationLength);
            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, spawnPositionZ);
            Coin newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity, this.transform).GetComponent<Coin>();
            if (newCoin != null)
            {
                newCoin.Init(scoreManager);                 
            }
        }
    }
}
