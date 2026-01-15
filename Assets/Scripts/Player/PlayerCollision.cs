using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float collisionCooldown = 1f;

    const string hitString = "Hit";

    float cooldownTimer = 0f;

    void Update()
    {
        cooldownTimer += Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Avoid triggering stumble animation again before completing an already playing one
        if (cooldownTimer < collisionCooldown) return;

        animator.SetTrigger(hitString);
        cooldownTimer = 0f;
    }
}
