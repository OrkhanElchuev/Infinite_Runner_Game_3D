using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] int startingChunksAmount = 12;
    [SerializeField] Transform chunkParent;
    [SerializeField] float chunkLength;
    [SerializeField] float moveSpeed = 8f;

    List<GameObject> chunks = new List<GameObject>();


    private void Start()
    {
        SpawnChunks();
    }

    void Update()
    {
        MoveChunks();
    }

    void SpawnChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++)
        {
            float spawnPositionZ = CalculateSpawnPositionZ(i);

            Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
            // Automatically child it to chunkParent
            GameObject newChunk = Instantiate(chunkPrefab, chunkSpawnPos, Quaternion.identity, chunkParent);

            chunks.Add(newChunk);
        }
    }

    float CalculateSpawnPositionZ(int i)
    {
        float spawnPositionZ;

        if (i == 0)
        {
            // 0, 0, 0
            spawnPositionZ = transform.position.z;
        }
        else
        {
            spawnPositionZ = transform.position.z + (i * chunkLength);
        }

        return spawnPositionZ;
    }

    void MoveChunks()
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            GameObject chunk = chunks[i];

            // Moving floor framerate independent 
            chunks[i].transform.Translate(-transform.forward * moveSpeed * Time.deltaTime);

            if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
            {
                chunks.Remove(chunk);
                Destroy(chunk);
            }
        }
    }
}
