using System;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] Transform chunkParent;

    [Header("Level Settings")]
    [SerializeField] int startingChunksAmount = 12;
    [Tooltip("Do not change chunk length value, without updating chunk size")]
    [SerializeField] float chunkLength;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float minMoveSpeed = 4f;
    [SerializeField] float maxMoveSpeed = 20f;
    [SerializeField] float minGravityZ = -22f;
    [SerializeField] float maxGravityZ = -2f;

    List<GameObject> chunks = new List<GameObject>();


    void Start()
    {
        SpawnStartingChunks();
    }

    void Update()
    {
        MoveChunks();
    }

    /* PUBLIC METHODS */
    public void ChangeChunkMoveSpeed(float speedAmount)
    {
        float newMoveSpeed = moveSpeed + speedAmount;
        // Make sure the speed doesnt go over or below the min and max values
        newMoveSpeed = Mathf.Clamp(newMoveSpeed, minMoveSpeed, maxMoveSpeed);

        if (newMoveSpeed < minMoveSpeed)
        {
            moveSpeed = minMoveSpeed;

            float newGravityZ = Physics.gravity.z - speedAmount;
            // Make sure the gravity doesnt go over or below the min and max values
            newGravityZ = Mathf.Clamp(newGravityZ, minGravityZ, maxGravityZ);
            
            // When speed is changed, adjust Z direction gravity, to avoid falling objects glitching
            Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGravityZ);
        }

        cameraController.ChangeCameraFOV(speedAmount);
    }

    /* PRIVATE METHODS*/
    void SpawnStartingChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++)
        {
            SpawnChunk();
        }
    }

    void SpawnChunk()
    {
        float spawnPositionZ = CalculateSpawnPositionZ();

        Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
        // Automatically child it to chunkParent
        GameObject newChunk = Instantiate(chunkPrefab, chunkSpawnPos, Quaternion.identity, chunkParent);

        chunks.Add(newChunk);
    }

    float CalculateSpawnPositionZ()
    {
        float spawnPositionZ;

        if (chunks.Count == 0)
        {
            // 0, 0, 0
            spawnPositionZ = transform.position.z;
        }
        else
        {
            spawnPositionZ = chunks[chunks.Count - 1].transform.position.z + chunkLength;
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
                SpawnChunk();
            }
        }
    }
}
