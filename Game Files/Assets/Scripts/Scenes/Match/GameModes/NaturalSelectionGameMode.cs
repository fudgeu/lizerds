using System;
using System.Collections;
using System.Collections.Generic;
using JoinMenu;
using UnityEngine;
using Object = UnityEngine.Object;

public class NaturalSelectionGameMode : MonoBehaviour {
    private List<GameObject> _players = new();
    private GameLifecycleManager _gameLifecycleManager;
    
    // Round state
    private int _alivePlayers;
    
    void Start()
    {
        // TODO subscribe to the scene load/unload events instead
        
        // Grab and process game lifecycle manager
        _gameLifecycleManager = GameObject.FindWithTag("GameLifecycleManager").GetComponent<GameLifecycleManager>();
        _gameLifecycleManager.OnRoundSetup += HandleOnRoundSetup;
        _gameLifecycleManager.OnRoundStart += HandleOnRoundStart;
        _gameLifecycleManager.OnRoundEnd += HandleOnRoundEnd;

        // Setup each player
        _players = GameObject.FindGameObjectWithTag("GameStartInfo").GetComponent<GameStartInfo>().players;
        _alivePlayers = _players.Count;
    }

    void OnDestroy()
    {
        _gameLifecycleManager.OnRoundSetup -= HandleOnRoundSetup;
    }

    void Update()
    {
        if (_gameLifecycleManager.RoundInSession && _alivePlayers <= 1)
        {
            // TODO temp disabled for demo
            _gameLifecycleManager.EndRound();
        }
    }
    
    private void HandleOnDeath()
    {
        _alivePlayers--;
    }

    private void HandleOnRoundSetup(int roundNumber, RoundLifecycleManager roundLifecycleManager)
    {
        roundLifecycleManager.useRoundTimer = true;
        roundLifecycleManager.roundTimer = 90;
        _alivePlayers = _players.Count;
    }

    private void HandleOnRoundStart(int roundNumber, RoundLifecycleManager roundLifecycleManager)
    {
        foreach (var player in _players)
        {
            // Register events
            player.GetComponentInChildren<PlayerController>().OnDeath += HandleOnDeath;
        }
    }

    private void HandleOnRoundEnd()
    {

    }
}
