using System.Collections;
using System.Collections.Generic;
using JoinMenu;
using UnityEngine;

public class StageStartManager : MonoBehaviour
{
    public GameObject gamePlayerPrefab;
    
    void Start()
    {
        // Get players
        var players = GameObject.FindGameObjectWithTag("GameStartInfo").GetComponent<GameStartInfo>().players;
        
        // Spawn players
        var spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        var usedSpawnPoints = new HashSet<GameObject>();
        
        foreach (var player in players)
        {
            // Create game player obj and set parent
            var gamePlayer = Instantiate(gamePlayerPrefab, player.transform);
            gamePlayer.transform.SetParent(player.transform);
            
            // Place at random spawn point
            GameObject spawnPoint = null;
            do
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            } while (usedSpawnPoints.Contains(spawnPoint));
            usedSpawnPoints.Add(spawnPoint);
            gamePlayer.transform.position = spawnPoint.transform.position;
            
            // Set up root properties
            player.GetComponent<PlayerRootController>().gamePlayerObject = gamePlayer;
        }
    }
}
