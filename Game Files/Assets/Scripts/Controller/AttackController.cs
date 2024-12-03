using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    [SerializeField] private float baseKnockback = 5f;      // Base knockback force
    [SerializeField] private float knockbackMultiplier = 0.1f; // Multiplier for HP knockback scaling
    [SerializeField] private float collisionCheckDuration = 0.5f; // Time for collision detection
    public LayerMask playerLayer;         // Ensure we only detect players
    [SerializeField] private Transform attackCheck; // Position of the attack check
    [SerializeField] private float attackCheckRadius = 0.5f; // Radius for the attack check

    private PlayerInput _inputAsset; // Reference to PlayerInput component
    private InputAction attack; // Reference to the attack action
    private bool isAttacking = false;

    private void Awake()
    {
        // Get the PlayerInput component from the parent
        _inputAsset = GetComponentInParent<PlayerInput>();

        // Get the current action map
        var playerInputActionMap = _inputAsset.currentActionMap;

        // Find the action for attacking
        attack = playerInputActionMap.FindAction("LightAttack");

        // Bind the action to the attack function
        attack.performed += DoAttack;

        // Enable the action
        attack.Enable();
    }

    private void OnDestroy()
    {
        attack.performed -= DoAttack;
    }

    private void DoAttack(InputAction.CallbackContext context)
    {
        if (isAttacking) return;

        // Determine the attack direction based on the lizard's facing direction
        Vector2 attackDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
        float attackForce = 10f; // Example: Attack force value
        PerformAttack(attackDirection, attackForce);
    }

    public void PerformAttack(Vector2 attackDirection, float attackForce)
    {
        if (isAttacking) return;
        StartCoroutine(CheckForHit(attackDirection, attackForce));
    }

    private IEnumerator CheckForHit(Vector2 attackDirection, float attackForce)
    {
        isAttacking = true;

        // Track already-hit players during this attack
        HashSet<Transform> hitPlayers = new HashSet<Transform>();

        // Enable hit detection temporarily
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius, playerLayer);
        foreach (Collider2D hit in hitColliders)
        {
            Transform root = hit.transform.root; // Find the root object of the hit player
            if (hitPlayers.Contains(root)) continue; // Skip if this player is already hit

            hitPlayers.Add(root); // Add this player to the set

            // Find the "MovementTarget" child object
            Transform movementTarget = root.GetChild(0)?.Find("MovementTarget");
            if (movementTarget == null)
            {
                Debug.LogWarning($"No MovementTarget found for {root.name}!");
                continue;
            }

            Rigidbody2D rb = movementTarget.GetComponent<Rigidbody2D>();
            PlayerHealth health = root.GetComponentInChildren<PlayerHealth>();

            if (health != null && rb != null)
            {
                // Calculate knockback
                float knockbackForce = attackForce + baseKnockback + (health.CurrentHP * knockbackMultiplier);
                rb.AddForce((-attackDirection).normalized * knockbackForce, ForceMode2D.Impulse);

                // Apply damage
                health.AddDamage(attackForce, transform); // Pass the attacker reference here
                Debug.Log($"Hit {root.name} for {attackForce} damage! {root.name} HP: {health.CurrentHP}%");
            }
        }

        yield return new WaitForSeconds(collisionCheckDuration);
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        // Visualize attack radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
}
