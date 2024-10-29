using System;
using System.Collections;
using System.Collections.Generic;
using JoinMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO merge with StageStartManager maybe?

// Manage the lifecycle of a round.
// For the most part, the game mode script will be
// doing the actual tracking of progress, and make
// calls here instead.

public class RoundLifecycleManager : MonoBehaviour {
    public GameLifecycleManager gameLifecycleManager;
    public bool useRoundTimer = true;
    public int roundTimer = 90;

    public bool timerRunning = false;
    private float _elapsedTime = 0;
    public float ElapsedTime => _elapsedTime;

    private Dictionary<GameObject, int> _scoreboard = new();
    
    void Start()
    {
        gameLifecycleManager.OnRoundStart += HandleOnRoundStart;
    }

    void FixedUpdate()
    {
        if (timerRunning) {
            _elapsedTime += Time.fixedDeltaTime;
            
            if (_elapsedTime < roundTimer || !useRoundTimer) return;
            timerRunning = false;
            _elapsedTime = roundTimer;
            gameLifecycleManager.EndRound();
        }
    }

    public void AdjustPlayerScore(GameObject player, int score)
    {
        _scoreboard[player] = score;
    }

    public Dictionary<GameObject, int> GetPlayerScoreboard()
    {
        return new Dictionary<GameObject, int>(_scoreboard); // Make copy so scores cannot be adjusted
    }

    private void HandleOnRoundStart(int roundNumber, RoundLifecycleManager _)
    {
        timerRunning = useRoundTimer;
    }
}
