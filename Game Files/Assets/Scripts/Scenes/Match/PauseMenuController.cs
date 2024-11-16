using JoinMenu;
using Scenes;
using Scenes.Match;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
            DisablePauseMenu();
        }
        else
        {
            // Enable pause menu
            _pauseMenuInstance = Instantiate(pauseMenuPrefab);
            var pauseMenuComponent = _pauseMenuInstance.GetComponentInChildren<PauseMenu>();
            pauseMenuComponent.ContinueButton.onClick.AddListener(OnClickContinue);
            pauseMenuComponent.ExitToMenuButton.onClick.AddListener(OnClickExit);
            Time.timeScale = 0;
            FindObjectOfType<EventSystem>().SetSelectedGameObject(pauseMenuComponent.ContinueButton.gameObject);
        }
    }

    private void DisablePauseMenu()
    {
        // Disable pause menu
        var pauseMenuComponent = _pauseMenuInstance.GetComponentInChildren<PauseMenu>();
        pauseMenuComponent.ContinueButton.onClick.RemoveAllListeners();
        pauseMenuComponent.ContinueButton.onClick.RemoveAllListeners();
        Destroy(_pauseMenuInstance);
        Time.timeScale = 1;
    }

    private void OnClickContinue()
    {
        DisablePauseMenu();
    }
    
    private void OnClickExit()
    {
        var director = GameObject.FindWithTag("LoadingScreenDirector").GetComponent<LoadingScreenDirector>();
        director.goTo = LoadingScreenDirector.GameScene.MainMenu;
        SceneManager.LoadScene("Loading");
    }
}
