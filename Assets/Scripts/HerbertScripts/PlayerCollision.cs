
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("Stress")]
    [SerializeField] private float collisionStress = 0.015f;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 30f;

    [Header("Collision")]
    [SerializeField] private float collisionCooldown = 0.6f;

    [Header("References")]
    [SerializeField] private StressBar stressBar;
    [SerializeField] private PlayerMovement playerMovement;

    private float lastCollisionTime = -Mathf.Infinity;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Obstacle"))
            return;

        if (Time.time < lastCollisionTime + collisionCooldown)
            return;

        lastCollisionTime = Time.time;

        ApplyCollisionEffects(other);
    }

    private void ApplyCollisionEffects(Collider other)
    {
        if (stressBar != null)
            stressBar.AddStress(collisionStress);

        Vector3 direction = transform.position - other.ClosestPoint(transform.position);

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            direction = transform.position - other.bounds.center;
            direction.y = 0f;
        }

        if (direction.sqrMagnitude < 0.001f)
            direction = -transform.forward;

        direction.Normalize();

        if (playerMovement != null)
        {
            playerMovement.ApplyKnockback(direction, knockbackForce);
        }

        Debug.Log("Player hit by: " + other.name);
    }
}
