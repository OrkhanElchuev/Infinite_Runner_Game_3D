using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Reads 2D movement input and moves a Rigidbody on the XZ plane.
/// Position is clamped so the player stays within bounds.
/// Intended for endless runner / lane-ish movement.
/// </summary>

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [Tooltip("Movement speed (units per second).")]
    [SerializeField] float moveSpeed = 5f;
    [Tooltip("Clamp limit on X axis (player stays between -xClamp and +xClamp).")]
    [SerializeField] float xClamp = 3f;
    [Tooltip("Clamp limit on Z axis (player stays between -zClamp and +zClamp).")]
    [SerializeField] float zClamp = 3f;

    Vector2 movement;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    void HandleMovement()
    {
        if (rb == null) return;

        // Compute movement direction on XZ plane.
        Vector3 moveDirection = new Vector3(movement.x, 0f, movement.y);

        // Move up and down, left and right on the surface.
        Vector3 currentPosition = rb.position;
        Vector3 newPosition = currentPosition + moveDirection * (moveSpeed * Time.fixedDeltaTime);

        // Clamp to play area bounds.
        newPosition.x = Mathf.Clamp(newPosition.x, -xClamp, xClamp);
        newPosition.z = Mathf.Clamp(newPosition.z, -zClamp, zClamp);

        // Move via Rigidbody for smoother physics interaction.
        rb.MovePosition(newPosition);
    }
}
