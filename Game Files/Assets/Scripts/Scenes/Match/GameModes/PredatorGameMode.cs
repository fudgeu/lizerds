using System;
using System.Collections;
using System.Collections.Generic;
using JoinMenu;
using Scenes.Match.GameModes;
using UnityEngine;
using Object = UnityEngine.Object;

public class PredatorGameMode : GameMode
{
    private List<GameObject> _players = new();
    private GameLifecycleManager _gameLifecycleManager;
    private RoundLifecycleManager _roundLifecycleManager;
    
    // Round state
    private List<GameObject> _deadPlayers = new();
    
    void Start()
    {
        gameModeName = "Predator";
        
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
    
    private void HandleOnDeath(GameObject player)
    {
        if (_deadPlayers.Contains(player)) return;
        print("Player died!");
        GameObject killer = null; // playerRoot.GetComponentInChildren<PlayerController>().lastAttacker;
        if (killer)
        {
            _roundLifecycleManager.AdjustPlayerScore(player.transform.parent.gameObject, _roundLifecycleManager.GetPlayerScore(killer) + 1);
        }
        _deadPlayers.Add(player);
        StartCoroutine(RespawnPlayer(player));
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
            var oobChecker = player.GetComponentInChildren<OutOfBoundsChecker>();
            oobChecker.onOOB += HandleOnDeath;
            oobChecker.respawnOnOOB = false;
            
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
        foreach (var rb in player.GetComponentsInChildren<Rigidbody2D>())
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0;
        }
        player.GetComponentInChildren<RespawnController>()?.RespawnPlayer();
    }
}
