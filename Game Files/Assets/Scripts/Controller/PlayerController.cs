using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //--Components--------------------
    [Header("Components")]
    [Tooltip("The Player Input component attached to this object")]
    [SerializeField] private PlayerInput inputAsset;

    private InputActionMap playerInputActionMap;

    [Tooltip("The rigidbody attached to this object")]
    [SerializeField] private new Rigidbody2D rigidbody;

    //--Input Actions-----------------
    private InputAction move;
    private InputAction leap;

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

    //--Respawn Position---------------
    [Header("Respawn Position")]
    [Tooltip("Where the player respawns when leaving the bounding box")]
    public Vector2 respawnPosition = Vector2.zero;

    private Vector2 currentVelocity; // For smooth acceleration and deceleration
    private bool isGrounded; // To track if the player is on the ground

    // Awake: Runs before Start() and OnEnable()
    private void Awake()
    {
        playerInputActionMap = inputAsset.currentActionMap;
    }

    // OnEnable: Runs when player prefab spawns
    private void OnEnable()
    {
        // Link and enable input actions
        move = playerInputActionMap.FindAction("Move");
        move.Enable();

        leap = playerInputActionMap.FindAction("Leap");
        leap.performed += DoLeap;
        leap.Enable();
    }

    // OnDisable: Unlink inputs when object is disabled
    private void OnDisable()
    {
        move.Disable();
        leap.Disable();
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

        // Check if player is out of bounds and respawn if necessary
        if (IsOutOfBounds())
        {
            Respawn();
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
    private void Respawn()
    {
        Debug.Log("Player out of bounds, respawning...");
        transform.position = respawnPosition;
        rigidbody.velocity = Vector2.zero;
    }

    // Visualize the bounding box in the scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 bottomLeft = new Vector3(boundingBoxMin.x, boundingBoxMin.y, 0);
        Vector3 topRight = new Vector3(boundingBoxMax.x, boundingBoxMax.y, 0);
        Gizmos.DrawWireCube((bottomLeft + topRight) / 2, topRight - bottomLeft);
    }
}