using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementTargetController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float leapForce = 5f;
    public bool isGrounded = false;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        Vector2 move = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        rb.velocity = move;

        // Flip the movement target when changing directions
        if (moveInput > 0.01f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Leap
        if (Input.GetButtonDown("Leap") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, leapForce);
        }

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
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
