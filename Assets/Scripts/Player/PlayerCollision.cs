using UnityEngine;

/// <summary>
/// Responds to player collisions by:
/// - triggering a "Hit" animation
/// - slowing down the level (negative speed delta)
/// Uses a cooldown so repeated collisions don't spam animation/speed changes.
/// </summary>

public class PlayerCollision : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Animator used to play the hit/stumble animation.")]
    [SerializeField] Animator animator;

    [Header("Collision Settings")]
    [Tooltip("Minimum time (seconds) between collision reactions.")]
    [SerializeField] float collisionCooldown = 1f;
    [Tooltip("Speed delta applied to LevelGenerator when collision happens (negative = slow down).")]
    [SerializeField] float adjustChangeMoveSpeedAmount = -2f;

    const string HIT_TRIGGER_STRING = "Hit";
    float cooldownTimer = 0f;

    LevelGenerator levelGenerator;

    void Start()
    {
        levelGenerator = FindFirstObjectByType<LevelGenerator>();
    }

    void Update()
    {
        // Track time since last collision reaction.
        cooldownTimer += Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Avoid triggering stumble animation again before completing an already playing one
        if (cooldownTimer < collisionCooldown) return;

        // Apply slowdown effect.
        if (levelGenerator != null)
        {
            levelGenerator.ChangeChunkMoveSpeed(adjustChangeMoveSpeedAmount);            
        }
        
        // Trigger animation (requires a "Hit" trigger parameter in Animator Controller).
        if (animator != null)
        {
            animator.SetTrigger(HIT_TRIGGER_STRING);            
        }

        cooldownTimer = 0f;
    }
}
