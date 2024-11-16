using JoinMenu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPrefab;
    private GameObject _pauseMenuInstance;
    
    
    void Start()
    {
        // Get all players and attach a button listener onto it
        var players = GameObject.FindWithTag("GameStartInfo").GetComponent<GameStartInfo>().players;
        foreach (var player in players)
        {
            var playerInput = player.GetComponent<PlayerInput>();
            playerInput.actions["Join"].performed += OnStartPressed;
        }
    }

    private void OnDestroy()
    {
        // Get all players and deattach button listener from it
        var players = GameObject.FindWithTag("GameStartInfo").GetComponent<GameStartInfo>().players;
        foreach (var player in players)
        {
            var playerInput = player.GetComponent<PlayerInput>();
            playerInput.actions["Join"].performed -= OnStartPressed;
        }
    }

    private void OnStartPressed(InputAction.CallbackContext ctx)
    {
        if (_pauseMenuInstance)
        {
            // Disable pause menu
            Destroy(_pauseMenuInstance);
            Time.timeScale = 1;
        }
        else
        {
            // Enable pause menu
            _pauseMenuInstance = Instantiate(pauseMenuPrefab);
            Time.timeScale = 0;
        }
    }
}
