using System;
using System.Collections;
using System.Collections.Generic;
using JoinMenu;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO merge with StageStartManager maybe?

// Manage the lifecycle of a round.
// For the most part, the game mode script will be
// doing the actual tracking of progress, and make
// calls here instead.

public class RoundLifecycleManager : MonoBehaviour {
    public GameLifecycleManager gameLifecycleManager;
    public GameObject timerTextPrefab;
    public bool useRoundTimer = true;
    public int roundTimer = 90;

    public bool timerRunning = false;
    private float _elapsedTime = 0;
    public float ElapsedTime => _elapsedTime;

    private Dictionary<GameObject, int> _scoreboard = new();
    private TMP_Text timerText;
    
    void Start()
    {
        gameLifecycleManager.OnRoundStart += HandleOnRoundStart;
    }

    private void OnDestroy()
    {
        gameLifecycleManager.OnRoundStart -= HandleOnRoundStart;
    }

    void FixedUpdate()
    {
        if (timerRunning) {
            _elapsedTime += Time.fixedDeltaTime;
            timerText.text = $"{Math.Round(roundTimer - _elapsedTime)}s";
            
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
    
    public int GetPlayerScore(GameObject player)
    {
        return _scoreboard[player];
    }

    private void HandleOnRoundStart(int roundNumber, RoundLifecycleManager _)
    {
        timerRunning = useRoundTimer;

        if (useRoundTimer)
        {
            var timerTextInst = Instantiate(timerTextPrefab);
            timerText = timerTextInst.GetComponentInChildren<TMP_Text>();
        }
    }
}
