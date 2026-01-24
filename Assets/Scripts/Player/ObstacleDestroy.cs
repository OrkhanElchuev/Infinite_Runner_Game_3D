using UnityEngine;

/// <summary>
/// Destroys any object that enters this trigger.
/// </summary>

public class ObstacleDestroy : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}
