using UnityEngine;

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
    private bool isFlipped = false;
    public float groundCheckRadius = 0.2f;

    [Tooltip("A second rigidbody to apply leap force to, to prevent backflipping")][SerializeField] private Rigidbody2D secondImportantJoint;

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
            bodyAnimation.ReleaseLegs();
            rb.velocity = new Vector2(rb.velocity.x, leapForce);
            secondImportantJoint.velocity = new Vector2(secondImportantJoint.velocity.x, leapForce * 0.8f);                                    //<- applied leap force to a second joint, preventing backflips
        }

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (!isGrounded)
        {
            isFlipped = Physics2D.OverlapCircle(backCheck.position, groundCheckRadius, groundLayer);
        }
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
