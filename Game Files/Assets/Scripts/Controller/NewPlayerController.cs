using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementTargetController : MonoBehaviour
{
    [SerializeField] private BodyAnimation bodyAnimation;       //Having body animation here isn't great software architecture, BUT we get her done. -Dyl
    public float moveSpeed = 10f;
    public float leapForce = 5f;
    public bool isGrounded = false;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Transform backCheck;
    public bool isFlipped = false;
    public float groundCheckRadius = 0.2f;
    [SerializeField] private float flipOverTorque = 50f;
    private Coroutine flipChecker;
    
    private PlayerInput _inputAsset;
    private InputActionMap playerInputActionMap;
    private InputAction move;
    private InputAction leap;

    [Tooltip("A second rigidbody to apply leap force to, to prevent backflipping")][SerializeField] private Rigidbody2D secondImportantJoint;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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
    }

    private void OnDisable()
    {
        _inputAsset = null;
        move.Disable();
        leap.Disable();
    }

    private void Update()
    {
        HandleMovement();
        HandleFlipping();
        CheckGroundStatus();
    }

    private void HandleMovement()
    {
        float moveInput = move.ReadValue<Vector2>().normalized.x;
        Vector2 calculatedMove = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        rb.velocity = calculatedMove;
    }

    private void HandleFlipping()
    {
        float moveInput = move.ReadValue<Vector2>().normalized.x;

        if (moveInput > 0.01f)
        {
            SetFlipDirection(-1);
        }
        else if (moveInput < -0.01f)
        {
            SetFlipDirection(1);
        }
    }

    private void SetFlipDirection(int direction)
    {
        transform.localScale = new Vector3(direction, 1, 1);
        if (isGrounded && bodyAnimation.flippedX != (direction == -1))
        {
            bodyAnimation.FlipX();
        }
    }

    private void DoLeap(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            bodyAnimation.ReleaseLegs();
            rb.velocity = new Vector2(rb.velocity.x, leapForce);
            if (secondImportantJoint != null)
            {
                secondImportantJoint.velocity = new Vector2(secondImportantJoint.velocity.x, leapForce * 0.8f);
            }
        }
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (!isGrounded)
        {
            isFlipped = Physics2D.OverlapCircle(backCheck.position, groundCheckRadius, groundLayer);
            if (flipChecker == null)
            {
                flipChecker = StartCoroutine(FlipBackOver());
            }
        }
    }


    private IEnumerator FlipBackOver()
    {
        Debug.Log("Attempting to flip over...");
        yield return new WaitForSeconds(1.5f);

        if (!isGrounded && isFlipped)
        {
            bodyAnimation.ReleaseLegs();
            //if(bodyAnimation.flippedX)
            //    GetComponent<FixedJoint2D>().connectedBody.AddTorque(-flipOverTorque, ForceMode2D.Impulse);
            //else
            GetComponent<FixedJoint2D>().connectedBody.AddTorque(flipOverTorque, ForceMode2D.Impulse);
            Debug.Log("FLIP");
        }
        isFlipped = false;
        flipChecker = null;
    }

    private void OnDrawGizmos()
    {
        // Visualize ground check
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
