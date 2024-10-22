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
            EndRound();
        }
    }

    private void HandleOnRoundStart(int roundNumber, RoundLifecycleManager _)
    {
        timerRunning = useRoundTimer;
    }
    
    private void EndRound()
    {
        gameLifecycleManager.EndRound();
    }
}
