using System;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] int startingChunksAmount = 12;
    [SerializeField] Transform chunkParent;
    [SerializeField] float chunkLength;
    [SerializeField] float moveSpeed = 8f;

    GameObject[] chunks = new GameObject[12];


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

            chunks[i] = newChunk;
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
        for (int i = 0; i < chunks.Length; i++)
        {
            // Moving floor framerate independent 
            chunks[i].transform.Translate(-transform.forward * moveSpeed * Time.deltaTime);
        }
    }
}
