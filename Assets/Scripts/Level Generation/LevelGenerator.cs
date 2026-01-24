using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns and scrolls level chunks to create an endless runner style track.
/// Also handles speed changes (and adjusts gravity & camera FOV accordingly).
/// </summary>

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Camera controller used to adjust FOV when speed changes.")]
    [SerializeField] CameraController cameraController;
    [Tooltip("Normal chunk prefabs randomly selected.")]
    [SerializeField] GameObject[] chunkPrefabs;
    [Tooltip("Checkpoint chunk prefab spawned every N chunks.")]
    [SerializeField] GameObject checkPointChunkPrefab;
    [Tooltip("Parent transform for all spawned chunks.")]
    [SerializeField] Transform chunkParent;
    [Tooltip("Score manager passed into chunks/coins.")]
    [SerializeField] ScoreManager scoreManager;

    [Header("Level Settings")]
    [Tooltip("How many chunks to spawn at level start.")]
    [SerializeField] int startingChunksAmount = 12;
    [Tooltip("Spawn a checkpoint chunk every N chunks.")]
    [SerializeField] int checkpointChunkInterval = 8;
    [Tooltip("Length of a chunk along Z; used for spawn spacing and despawn threshold.")]
    [SerializeField] float chunkLength;
    [Tooltip("How fast chunks move backward each second.")]
    [SerializeField] float moveSpeed = 8f;
    [Tooltip("Minimum allowed move speed.")]
    [SerializeField] float minMoveSpeed = 4f;
    [Tooltip("Maximum allowed move speed.")]
    [SerializeField] float maxMoveSpeed = 20f;
    [Tooltip("Minimum allowed gravity.z while speeding up.")]
    [SerializeField] float minGravityZ = -22f;
    [Tooltip("Maximum allowed gravity.z while slowing down.")]
    [SerializeField] float maxGravityZ = -2f;

    List<GameObject> chunks = new List<GameObject>();
    int chunksSpawned = 0;

    void Start()
    {
        SpawnStartingChunks();
    }

    void Update()
    {
        MoveChunks();
    }

    /* PUBLIC METHODS */

    // Adjust chunk scrolling speed and also adjusts gravity.z to reduce physics jitter 
    // for falling objects when the world speed changes. Adjust Camera FOV.
    public void ChangeChunkMoveSpeed(float speedAmount)
    {
        // Clamp speed within allowed bounds.
        float newMoveSpeed = Mathf.Clamp(moveSpeed + speedAmount, minMoveSpeed, maxMoveSpeed);

        if (newMoveSpeed != moveSpeed)
        {
            moveSpeed = newMoveSpeed;

            // Increase/Decrease gravity.z opposite to speed change to keep falling objects stable.
            float newGravityZ = Mathf.Clamp(Physics.gravity.z - speedAmount, minGravityZ, maxGravityZ);
            Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGravityZ);
        }

        // Camera effect is independent (you may want FOV changes even when clamped).
        if (cameraController != null)
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

        GameObject chunkToSpawn = ChooseChunkToSpawn();

        // Automatically child it to chunkParent.
        GameObject newChunkGameObj = Instantiate(chunkToSpawn, chunkSpawnPos, Quaternion.identity, chunkParent);
        chunks.Add(newChunkGameObj);

        // Provide chunk with the references it needs for spawned items.
        Chunk newChunk = newChunkGameObj.GetComponent<Chunk>();
        if (newChunk != null)
            newChunk.Init(this, scoreManager);


        chunksSpawned++;
    }

    private GameObject ChooseChunkToSpawn()
    {
        GameObject chunkToSpawn;

        if (chunksSpawned % checkpointChunkInterval == 0 && chunksSpawned != 0)
        {
            chunkToSpawn = checkPointChunkPrefab;
        }
        else
        {
            // Choose randomly from the list of available chunks
            chunkToSpawn = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
        }

        return chunkToSpawn;
    }

    float CalculateSpawnPositionZ()
    {
        if (chunks.Count == 0)
        {
            // First chunk starts at generator position.
            return transform.position.z;
        }

        // Spawn after the last chunk.
        return chunks[chunks.Count - 1].transform.position.z + chunkLength;
    }

    void MoveChunks()
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            GameObject chunk = chunks[i];

            // Moving floor framerate independent.
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
