using System;
using System.Collections;
using System.Collections.Generic;
using JoinMenu;
using Scenes.Match.GameModes;
using UnityEngine;
using Object = UnityEngine.Object;

public class NaturalSelectionGameMode : GameMode
{
    private List<GameObject> _players = new();
    private GameLifecycleManager _gameLifecycleManager;
    private RoundLifecycleManager _roundLifecycleManager;
    
    // Round state
    private int _initialPlayers;
    private int _alivePlayers;
    private List<GameObject> deadPlayers = new();
    
    void Start()
    {
        gameModeName = "Natural Selection";
        
        // Grab and process game lifecycle manager
        _gameLifecycleManager = GameObject.FindWithTag("GameLifecycleManager").GetComponent<GameLifecycleManager>();
        _gameLifecycleManager.OnRoundSetup += HandleOnRoundSetup;
        _gameLifecycleManager.OnRoundStart += HandleOnRoundStart;
        _gameLifecycleManager.OnRoundEnd += HandleOnRoundEnd;

        // Setup each player
        _players = GameObject.FindGameObjectWithTag("GameStartInfo").GetComponent<GameStartInfo>().players;
        _alivePlayers = _players.Count;
        _initialPlayers = _players.Count;
        print($"Initial players: {_initialPlayers}");
    }

    void OnDestroy()
    {
        _gameLifecycleManager.OnRoundSetup -= HandleOnRoundSetup;
    }

    void Update()
    {
        if (_gameLifecycleManager.RoundInSession && _alivePlayers <= 1)
        {
            _gameLifecycleManager.EndRound();
        }
    }
    
    private void HandleOnDeath(GameObject player)
    {
        if (deadPlayers.Contains(player)) return;
        print("Player died!");
        _roundLifecycleManager.AdjustPlayerScore(player.transform.parent.gameObject, _initialPlayers - _alivePlayers);
        _alivePlayers--;
        deadPlayers.Add(player);
    }

    private void HandleOnRoundSetup(int roundNumber, RoundLifecycleManager roundLifecycleManager)
    {
        _roundLifecycleManager = roundLifecycleManager;
        _roundLifecycleManager.useRoundTimer = true;
        _roundLifecycleManager.roundTimer = 90;
        _alivePlayers = _players.Count;
    }

    private void HandleOnRoundStart(int roundNumber, RoundLifecycleManager roundLifecycleManager)
    {
        _roundLifecycleManager = roundLifecycleManager;
        foreach (var player in _players)
        {
            // Register events
            var oobChecker = player.GetComponentInChildren<OutOfBoundsChecker>();
            oobChecker.onOOB += HandleOnDeath;
            oobChecker.respawnOnOOB = false;
            
            // Set up initial scores
            _roundLifecycleManager.AdjustPlayerScore(player, _alivePlayers - 1);
        }
    }

    private void HandleOnRoundEnd()
    {

    }
}
