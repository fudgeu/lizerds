using System;
using System.Collections;
using System.Collections.Generic;
using JoinMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manage the lifecycle of a round.
// For the most part, the game mode script will be
// doing the actual tracking of progress, and make
// calls here instead.

public class RoundLifecycleManager : MonoBehaviour {
    public bool useRoundTimer = true;
    public int roundTimer = 90;

    public bool timerRunning = false;
    private float _elapsedTime = 0;
    public float ElapsedTime => _elapsedTime;
    
    private GameStartInfo _gameStartInfo;

    void Start()
    {
        timerRunning = useRoundTimer;
        _gameStartInfo = GameObject.Find("GameStartInfo").GetComponent<GameStartInfo>();
    }

    void FixedUpdate()
    {
        if (timerRunning) {
            _elapsedTime += Time.fixedDeltaTime;
            
            if (_elapsedTime < roundTimer || !useRoundTimer) return;
            timerRunning = false;
            _elapsedTime = roundTimer;
            EndRound();
        }
    }


    public void EndRound()
    {
        // Destroy all game players
        foreach (var player in _gameStartInfo.players)
        {
            var rootController = player.GetComponent<PlayerRootController>();
            Destroy(rootController.gamePlayerObject);
            rootController.gamePlayerObject = null;
        }
        
        // Go to mutation phase
        SceneManager.LoadScene("Mutation");
    }
}
