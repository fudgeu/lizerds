using System.Collections;
using System.Collections.Generic;
using JoinMenu;
using Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MutationSceneManager : MonoBehaviour
{
    private GameStartInfo _gameStartInfo;
    private GameLifecycleManager _gameLifecycleManager;
    
    void Start()
    {
        _gameStartInfo = GameObject.FindWithTag("GameStartInfo").GetComponent<GameStartInfo>();
        _gameLifecycleManager = GameObject.FindWithTag("GameLifecycleManager").GetComponent<GameLifecycleManager>();
        
        // Get all players and automatically add button listener script
        foreach (var player in _gameStartInfo.players)
        {
            var buttonListenerScript = player.AddComponent<MutationPlayerButtonListener>();
            buttonListenerScript.mutationSceneManager = this;
        }
    }

    public void OnPlayerPressStart()
    {
        // Remove button listener scripts
        foreach (var player in _gameStartInfo.players)
        {
            Destroy(player.GetComponent<MutationPlayerButtonListener>());
        }
        
        _gameLifecycleManager.StartRound();
    }
}
