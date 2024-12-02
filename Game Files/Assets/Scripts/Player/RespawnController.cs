using UnityEngine;

public class RespawnController : MonoBehaviour
{
    [SerializeField] private Transform player;        // Reference to the player
    [SerializeField] private Transform spawnPoint;    // Spawn point where the player will respawn
    [SerializeField] private ParticleSystem respawnEffect; // Optional: Respawn particle effect

    public void RespawnPlayer()
    {
        if (player == null || spawnPoint == null) return;

        // Optionally play a respawn effect
        if (respawnEffect != null)
        {
            Instantiate(respawnEffect, spawnPoint.position, Quaternion.identity);
        }

        // Move player to the spawn point
        player.position = spawnPoint.position;

        Debug.Log("Player respawned at the spawn point!");
    }
}
