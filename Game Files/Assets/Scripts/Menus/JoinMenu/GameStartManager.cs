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
            var gameStartInfoObj = new GameObject("GameStartInfo");
            var gameStartInfo = gameStartInfoObj.AddComponent<GameStartInfo>();
            DontDestroyOnLoad(gameStartInfoObj);
            gameStartInfoObj.tag = "GameStartInfo";
            
            // Prepare players
            foreach (var player in _registeredPlayers)
            {
                var playerRoot = player.transform.parent.gameObject;
                DontDestroyOnLoad(playerRoot);
                gameStartInfo.players.Add(playerRoot);
                playerRoot.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            }
            _registeredPlayers.ForEach(Destroy);
            
            // Set up loading screen director
            var director = GameObject.FindWithTag("LoadingScreenDirector")?.GetComponent<LoadingScreenDirector>();
            if (director is null)
            {
                var directorObj = new GameObject("LoadingScreenDirector");
                directorObj.tag = "LoadingScreenDirector";
                director = directorObj.AddComponent<LoadingScreenDirector>();
            }
            
            DontDestroyOnLoad(director.gameObject);
            director.goTo = LoadingScreenDirector.GameScene.Arena;
            
            // Load arena scene
            DontDestroyOnLoad(playerInputManager.gameObject);
            SceneManager.LoadScene("Loading");
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
