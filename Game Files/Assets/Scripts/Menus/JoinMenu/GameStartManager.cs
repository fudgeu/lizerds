using System;
using System.Collections;
using System.Collections.Generic;
using JoinMenu;
using Scenes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviour
{
    // Settings
    public int playersRequiredToStart = 2;
    
    // UI objects
    public GameObject holdToStartText;
    public RectTransform startBarBackground;
    public RectTransform startBar;

    public PlayerInputManager playerInputManager;
    public GameObject gameLifecycleManagerPrefab;
    
    private List<GameObject> _registeredPlayers = new();
    
    // Hold start mechanic
    private GameObject _playerHoldingStart = null;
    private bool _isHoldingStart = false;
    private double _holdDuration = 0d;
    private double _holdThreshold = 1.5d;

    public void FixedUpdate()
    {
        // Add onto hold timer
        if (_isHoldingStart)
        {
            _holdDuration += Time.fixedDeltaTime;
            _holdDuration = Math.Clamp(_holdDuration, 0.0, _holdThreshold);
        }
        
        // Update start bar UI
        startBar.offsetMax = new Vector2(
            -startBarBackground.rect.width + ((float)(_holdDuration / _holdThreshold) * startBarBackground.rect.width),
            startBar.offsetMax.y
        );

        // Start game if hold duration exceeds threshold
        if (_holdDuration >= _holdThreshold && _registeredPlayers.Count >= playersRequiredToStart)
        {
            // Create game object that will contain all the player data
            GameObject gameStartInfoObj = GameObject.FindWithTag("GameStartInfo");
            if (gameStartInfoObj is null)
            {
                gameStartInfoObj = new GameObject("GameStartInfo");
                gameStartInfoObj.AddComponent<GameStartInfo>();
                DontDestroyOnLoad(gameStartInfoObj);
                gameStartInfoObj.tag = "GameStartInfo";
            }
            var gameStartInfo = gameStartInfoObj.GetComponent<GameStartInfo>();
            
            // Prepare players
            foreach (var player in _registeredPlayers)
            {
                var playerRoot = player.transform.parent.gameObject;
                DontDestroyOnLoad(playerRoot);
                gameStartInfo.players.Add(playerRoot);
                playerRoot.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            }
            _registeredPlayers.ForEach(Destroy);

            DontDestroyOnLoad(playerInputManager.gameObject);
            
            // Set up game lifecycle manager and start round
            var gameManagerObj = Instantiate(gameLifecycleManagerPrefab);
            DontDestroyOnLoad(gameManagerObj);
            
            var gameLifecycleManager = gameManagerObj.GetComponent<GameLifecycleManager>();
            gameLifecycleManager.StartRound();
        }
    }

    public void registerPlayer(GameObject player)
    {
        _registeredPlayers.Add(player);
        onPlayerChangeOccured();
    }

    public void unregisterPlayer(GameObject player)
    {
        _registeredPlayers.Remove(player);
        onPlayerChangeOccured();
    }

    public void onPlayerHoldStart(GameObject player)
    {
        if (_isHoldingStart) return;
        if (_registeredPlayers.Count < playersRequiredToStart) return;
        _isHoldingStart = true;
        _playerHoldingStart = player;
        _holdDuration = 0;
        startBarBackground.gameObject.SetActive(true);
    }

    public void onPlayerLetGoStart(GameObject player)
    {
        if (player != _playerHoldingStart) return;
        _isHoldingStart = false;
        _playerHoldingStart = null;
        _holdDuration = 0;
        startBarBackground.gameObject.SetActive(false);
    }

    private void onPlayerChangeOccured()
    {
        if (_registeredPlayers.Count >= playersRequiredToStart)
        {
            // Enable starting
            holdToStartText.SetActive(true);
        }
        else
        {
            // Disable starting
            holdToStartText.SetActive(false);
        }
    }
}
