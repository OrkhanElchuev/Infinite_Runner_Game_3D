using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] GameObject fencePrefab;
    [SerializeField] float[] lanes = {-2.5f, 0f, 2.5f};
    
    void Start()
    {
        SpawnFence();
    }

    void SpawnFence()
    {
        List<int> availableLanes = new List<int> {0, 1, 2};
        int fencesToSpawn = Random.Range(0, lanes.Length);
        // Make sure there is always at least 1 available lane for a player
        for (int i = 0; i < fencesToSpawn; i++)
        {
            if (availableLanes.Count <= 0) break;

            // Find and select an available lane without an already instantiated fence
            int randomLaneIndex = Random.Range(0, availableLanes.Count);
            int selectedLane = availableLanes[randomLaneIndex];
            // Remove it from the list to not instantiate another fence on the same lane
            availableLanes.RemoveAt(randomLaneIndex);

            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
            Instantiate(fencePrefab, spawnPosition, Quaternion.identity, this.transform);
        }
    }
}
