using UnityEngine;

/// <summary>
/// Base class for pickups:
/// - spins the object for visibility
/// - detects Player trigger enter
/// - calls OnPickup() for derived behavior
/// - destroys itself afterwards
/// Derive from this and implement OnPickup().
/// </summary>

public abstract class PickUp : MonoBehaviour
{
    [Tooltip("Rotation speed in degrees per second.")]
    [SerializeField] float rotationSpeed = 100f;

    const string PLAYER_STRING = "Player";

    void Update()
    {
        // Rotate around Y axis so pickups are noticeable.
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        // Only react to the player..
        if (!other.CompareTag(PLAYER_STRING)) return;

        OnPickup();
        Destroy(gameObject);  
    }

    protected abstract void OnPickup();
}
