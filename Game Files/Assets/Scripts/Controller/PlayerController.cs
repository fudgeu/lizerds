using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //--Components--------------------
    [Header("Components")]
    // [Tooltip("The Player Input component attached to this object")]
    private PlayerInput _inputAsset;

    private InputActionMap playerInputActionMap;

    [Tooltip("The rigidbody attached to this object")]
    [SerializeField] private new Rigidbody2D rigidbody;

    //--Input Actions-----------------
    private InputAction move;
    private InputAction leap;
    private InputAction lightAttack;
    private InputAction heavyAttack;

    //--Movement Configuration--------
    [Header("Movement Configuration")]
    [Tooltip("The player's maximum movement speed")]
    public float moveSpeed;
    [Tooltip("The rate the player accelerates at.")]
    [SerializeField] private float acceleration;
    [Tooltip("The rate the player decelerates at.")]
    [SerializeField] private float deceleration;

    [Tooltip("The force applied in the target direction when leap is used")]
    public Vector2 leapForce;

    //--Ground Detection---------------
    [Header("Ground Detection")]
    [Tooltip("The layer used for ground detection")]
    [SerializeField] private LayerMask groundLayer;
    [Tooltip("The distance to check for ground")]
    [SerializeField] private float groundCheckDistance = 0.1f;

    //--Bounding Box------------------
    [Header("Bounding Box")]
    [Tooltip("Min X and Y for the bounding box")]
    public Vector2 boundingBoxMin = new Vector2(-10, -5);
    
    [Tooltip("Max X and Y for the bounding box")]
    public Vector2 boundingBoxMax = new Vector2(10, 5);

    //--Combat Config-----------------
    [Tooltip("Attack range of the player")]
    [SerializeField] private float attackRange = 1.0f;

    [Tooltip("Layers that can be hit by the player")]
    [SerializeField] private LayerMask enemyLayer;
    [Header("Attack Configuration")]
    [Tooltip("The total force applied on hit")]
    [SerializeField] private float hitForce = 10f; // The total hit force

    //--Respawn Position---------------
    [Header("Respawn Position")]
    [Tooltip("Where the player respawns when leaving the bounding box")]
    public Vector2 respawnPosition = Vector2.zero;

    //--Events-------------------------
    public delegate void PlayerEvent(GameObject playerRoot, GameObject gamePlayer);
    public event PlayerEvent OnDeath;
    
    private Vector2 currentVelocity; // For smooth acceleration and deceleration
    private bool isGrounded; // To track if the player is on the ground
    
    //--Attack Info--------------------
    public GameObject lastAttacker;

    // Awake: Runs before Start() and OnEnable()
    private void Awake()
    {
    }

    // OnEnable: Link attacks to input actions
    private void OnEnable()
    {
        // Get player input
        _inputAsset = GetComponentInParent<PlayerInput>();
        playerInputActionMap = _inputAsset.currentActionMap;
        
        move = playerInputActionMap.FindAction("Move");
        move.Enable();

        leap = playerInputActionMap.FindAction("Leap");
        leap.performed += DoLeap;
        leap.Enable();

        // Light Attack
        lightAttack = playerInputActionMap.FindAction("LightAttack");
        lightAttack.performed += _ => LightAttack();
        lightAttack.Enable();

        // Heavy Attack
        heavyAttack = playerInputActionMap.FindAction("HeavyAttack");
        heavyAttack.performed += _ => HeavyAttack();
        heavyAttack.Enable();
    }

    // OnDisable: Unlink inputs when object is disabled
    private void OnDisable()
    {
        _inputAsset = null;
        move.Disable();
        leap.Disable();
        lightAttack.Disable();
        heavyAttack.Disable();
    }

    // DoLeap: Send player in their look direction + small upward movement (unless they are trying to leap down)
    private void DoLeap(InputAction.CallbackContext context)
    {
        // Only allow jumping if the player is grounded
        if (isGrounded)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0); // Reset vertical velocity before jumping
            rigidbody.AddForce(leapForce, ForceMode2D.Impulse);
            Debug.Log("LEAPING!");
        }
    }

    // Light Attack: Quick attack that deals small damage
    private void LightAttack()
    {
        // Perform the attack with a short range and small damage
        Attack(0.5f);
        Debug.Log("LIGHT ATTACK!");
    }

    // Heavy Attack: Slower but deals more damage
    private void HeavyAttack()
    {
        // Perform the attack with a longer range and more damage
        Attack(1.0f);
        Debug.Log("HEAVY ATTACK!");
    }

    private void Attack(float forcePercentage)
        {
            // Create a circle around the player to check for enemies (including players)
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

            // Get all the colliders for the player's child segments
            List<Collider2D> playerColliders = new List<Collider2D>(GetComponentsInChildren<Collider2D>());

            // Damage the enemies in range, but skip the player and its own child segments
            foreach (Collider2D enemy in hitEnemies)
            {
                if (!playerColliders.Contains(enemy)) // Exclude player and child segments
                {
                    Debug.Log("Hit " + enemy.name);
                    // Apply damage to the enemy (implement actual damage logic here)

                    // Calculate the direction from the player to the enemy
                    Vector2 hitDirection = (enemy.transform.position - transform.position).normalized;

                    // Calculate the force to apply based on the attack type
                    Vector2 forceToApply = hitDirection * hitForce * forcePercentage;

                    // Get the Rigidbody2D component from the enemy (dummy)
                    Rigidbody2D enemyRigidbody = enemy.GetComponent<Rigidbody2D>();
                    if (enemyRigidbody != null)
                    {
                        enemyRigidbody.AddForce(forceToApply, ForceMode2D.Impulse);
                    }
                    
                    // Set enemy's last attacker
                    PlayerController enemyPlayerController = enemy.GetComponent<PlayerController>();
                    enemyPlayerController.lastAttacker = gameObject.transform.parent.gameObject;
                }
            }
        }

    private void FixedUpdate()
    {
        // Read input for movement
        Vector2 input = move.ReadValue<Vector2>();
        
        // Calculate target velocity based on input
        Vector2 targetVelocity = new Vector2(input.x * moveSpeed, rigidbody.velocity.y);

        // Apply acceleration or deceleration based on current velocity
        if (input.x != 0)
        {
            // Accelerate towards target velocity
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, targetVelocity.x, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // Decelerate when no input is present
            currentVelocity.x = Mathf.MoveTowards(currentVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }

        // Set the player's horizontal velocity directly
        rigidbody.velocity = new Vector2(currentVelocity.x, rigidbody.velocity.y);

        // Check if player is grounded
        CheckGrounded();

        // Check if player is out of bounds
        if (IsOutOfBounds())
        {
            OnDeath?.Invoke(transform.parent.gameObject, gameObject);
        }
    }

    // Check if the player is on the ground
    private void CheckGrounded()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    // Check if the player is outside the bounding box
    private bool IsOutOfBounds()
    {
        Vector2 position = transform.position;
        return (position.x < boundingBoxMin.x || position.x > boundingBoxMax.x ||
                position.y < boundingBoxMin.y || position.y > boundingBoxMax.y);
    }

    // Respawn the player at the center
    public void Respawn()
    {
        transform.position = respawnPosition;
        rigidbody.velocity = Vector2.zero;
    }

    // Visualize the bounding box in the scene view
    private void OnDrawGizmos()
    {
        // Visualize the bounding box
        Gizmos.color = Color.red;
        Vector3 bottomLeft = new Vector3(boundingBoxMin.x, boundingBoxMin.y, 0);
        Vector3 topRight = new Vector3(boundingBoxMax.x, boundingBoxMax.y, 0);
        Gizmos.DrawWireCube((bottomLeft + topRight) / 2, topRight - bottomLeft);

        // Visualize the attack range
        Gizmos.color = Color.red; // You can change the color if needed
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}