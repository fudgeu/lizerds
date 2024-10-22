using System.Collections;
using System.Collections.Generic;
using JoinMenu;
using Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MutationSceneManager : MonoBehaviour
{
    private GameStartInfo _gameStartInfo;
    
    void Start()
    {
        _gameStartInfo = GameObject.FindWithTag("GameStartInfo").GetComponent<GameStartInfo>();
        
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
        
        // Set up loading screen to switch back to arena
        var loadingScreenDirector = GameObject.FindWithTag("LoadingScreenDirector").GetComponent<LoadingScreenDirector>();
        if (loadingScreenDirector is null)
        {
            var directorGameObj = new GameObject();
            DontDestroyOnLoad(directorGameObj);
            loadingScreenDirector = directorGameObj.AddComponent<LoadingScreenDirector>();
        }
        
        loadingScreenDirector.goTo = LoadingScreenDirector.GameScene.Arena;
        SceneManager.LoadScene("Loading");
    }
}
