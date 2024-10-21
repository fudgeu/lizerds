using System;
using System.Collections;
using System.Collections.Generic;
using JoinMenu;
using UnityEngine;
using Object = UnityEngine.Object;

public class NaturalSelectionGameMode : MonoBehaviour {
    private RoundLifecycleManager _roundLifecycleManager;
    private List<GameObject> _players = new();
    
    // Round state
    private int _alivePlayers;
    
    void Start()
    {
        // TODO subscribe to the scene load/unload events instead
        
        // Setup round lifecycle manager
        _roundLifecycleManager = GameObject.FindWithTag("StageManager").GetComponent<RoundLifecycleManager>();
        _roundLifecycleManager.useRoundTimer = true;
        _roundLifecycleManager.roundTimer = 90;

        // Setup each player
        foreach (var player in GameObject.FindGameObjectWithTag("GameStartInfo").GetComponent<GameStartInfo>().players)
        {
            // Get player and add to list
            var gamePlayer = player.GetComponent<PlayerRootController>().gamePlayerObject;
            _players.Add(gamePlayer);
            
            // Register events
            player.GetComponentInChildren<PlayerController>().OnDeath += HandleOnDeath;
        }
        
        _alivePlayers = _players.Count;
    }

    void Update()
    {
        if (_alivePlayers <= 1)
        {
            _roundLifecycleManager.EndRound();
        }
    }
    
    private void HandleOnDeath()
    {
        _alivePlayers--;
    }
}
