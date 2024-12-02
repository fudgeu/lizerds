using UnityEngine;

public class OutOfBoundsChecker : MonoBehaviour
{
    [SerializeField] private Transform player;        // Reference to the player
    [SerializeField] private Vector2 boundsMin;       // Minimum bounds (e.g., bottom-left corner)
    [SerializeField] private Vector2 boundsMax;       // Maximum bounds (e.g., top-right corner)
    [SerializeField] private RespawnController respawnController; // Reference to the RespawnController script

    private void Update()
    {
        if (player == null || respawnController == null) return;

        // Check if player is out of bounds
        if (player.position.x < boundsMin.x || player.position.x > boundsMax.x ||
            player.position.y < boundsMin.y || player.position.y > boundsMax.y)
        {
            Debug.Log("Player is out of bounds!");
            respawnController.RespawnPlayer();
        }
    }

    private void OnDrawGizmos()
    {
        // Draw a rectangle in the scene view to visualize the bounds
        Gizmos.color = Color.red;
        Vector3 boundsCenter = new Vector3((boundsMin.x + boundsMax.x) / 2, (boundsMin.y + boundsMax.y) / 2, 0);
        Vector3 boundsSize = new Vector3(boundsMax.x - boundsMin.x, boundsMax.y - boundsMin.y, 0);
        Gizmos.DrawWireCube(boundsCenter, boundsSize);
    }
}
