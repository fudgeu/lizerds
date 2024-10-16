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
        foreach (var player in players)
        {
            // Create game player obj and set parent
            var gamePlayer = Instantiate(gamePlayerPrefab, player.transform);
            gamePlayer.transform.SetParent(player.transform);
            
            // Set up
            player.GetComponent<PlayerRootController>().gamePlayerObject = gamePlayer;
        }
    }
}
