using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    // Reference to the Rigidbody2D component
    private Rigidbody2D rb;

    // Bounding box settings
    [Header("Bounding Box")]
    [Tooltip("Min X and Y for the bounding box")]
    public Vector2 boundingBoxMin = new Vector2(-10, -5);
    
    [Tooltip("Max X and Y for the bounding box")]
    public Vector2 boundingBoxMax = new Vector2(10, 5);

    private void Start()
    {
        // Get the Rigidbody2D component attached to the dummy
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check if the dummy is out of bounds and respawn if necessary
        if (IsOutOfBounds())
        {
            StartCoroutine(RespawnCoroutine());
        }
    }

    // Check if the dummy is outside the bounding box
    private bool IsOutOfBounds()
    {
        Vector2 position = transform.position;
        return (position.x < boundingBoxMin.x || position.x > boundingBoxMax.x ||
                position.y < boundingBoxMin.y || position.y > boundingBoxMax.y);
    }

    // Coroutine for respawning the dummy after a delay
    private IEnumerator RespawnCoroutine()
    {
        // Wait for 3 seconds before respawning
        yield return new WaitForSeconds(3f);

        Debug.Log("Dummy out of bounds, respawning...");

        // Reset position to the center of the bounding box
        transform.position = new Vector2((boundingBoxMin.x + boundingBoxMax.x) / 2, 
                                          (boundingBoxMin.y + boundingBoxMax.y) / 2);

        // Reset the dummy's velocity to zero
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
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