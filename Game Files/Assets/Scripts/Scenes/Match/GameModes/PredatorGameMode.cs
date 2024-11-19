using System;
using System.Collections;
using System.Collections.Generic;
using JoinMenu;
using UnityEngine;
using Object = UnityEngine.Object;

public class PredatorGameMode : MonoBehaviour {
    private List<GameObject> _players = new();
    private GameLifecycleManager _gameLifecycleManager;
    private RoundLifecycleManager _roundLifecycleManager;
    
    // Round state
    private List<GameObject> _deadPlayers = new();
    
    void Start()
    {
        // TODO subscribe to the scene load/unload events instead
        
        // Grab and process game lifecycle manager
        _gameLifecycleManager = GameObject.FindWithTag("GameLifecycleManager").GetComponent<GameLifecycleManager>();
        _gameLifecycleManager.OnRoundSetup += HandleOnRoundSetup;
        _gameLifecycleManager.OnRoundStart += HandleOnRoundStart;
        _gameLifecycleManager.OnRoundEnd += HandleOnRoundEnd;
        
        _players = GameObject.FindGameObjectWithTag("GameStartInfo").GetComponent<GameStartInfo>().players;
    }

    void OnDestroy()
    {
        _gameLifecycleManager.OnRoundSetup -= HandleOnRoundSetup;
    }
    
    private void HandleOnDeath(GameObject playerRoot, GameObject _)
    {
        if (_deadPlayers.Contains(playerRoot)) return;
        print("Player died!");
        var killer = playerRoot.GetComponentInChildren<PlayerController>().lastAttacker;
        if (killer)
        {
            _roundLifecycleManager.AdjustPlayerScore(playerRoot, _roundLifecycleManager.GetPlayerScore(killer) + 1);
        }
        _deadPlayers.Add(playerRoot);
        StartCoroutine(RespawnPlayer(playerRoot));
    }

    private void HandleOnRoundSetup(int roundNumber, RoundLifecycleManager roundLifecycleManager)
    {
        _roundLifecycleManager = roundLifecycleManager;
        _roundLifecycleManager.useRoundTimer = true;
        _roundLifecycleManager.roundTimer = 90;
    }

    private void HandleOnRoundStart(int roundNumber, RoundLifecycleManager roundLifecycleManager)
    {
        _roundLifecycleManager = roundLifecycleManager;
        foreach (var player in _players)
        {
            // Register events
            player.GetComponentInChildren<PlayerController>().OnDeath += HandleOnDeath;
            
            // Set up initial scores
            _roundLifecycleManager.AdjustPlayerScore(player, 0);
        }
    }

    private void HandleOnRoundEnd()
    {

    }

    private IEnumerator RespawnPlayer(GameObject player)
    {
        yield return new WaitForSeconds(3);
        _deadPlayers.Remove(player);
        player.GetComponentInChildren<PlayerController>()?.Respawn();
    }
}
