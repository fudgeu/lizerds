using JoinMenu;
using Scenes;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsScreenScoreboardManager : MonoBehaviour, ButtonManager
{
    private GameStartInfo _gameStartInfo;
    
    void Start()
    {
        var resultsInfo = FindObjectOfType<ResultsInfo>();
        GetComponent<EndRoundScoreboardSetup>().roundScoreboard = resultsInfo.gameScoreboard;
        
        _gameStartInfo = GameObject.FindWithTag("GameStartInfo").GetComponent<GameStartInfo>();
        
        // Get all players and automatically add button listener script
        foreach (var player in _gameStartInfo.players)
        {
            var buttonListenerScript = player.AddComponent<PlayerButtonListener>();
            buttonListenerScript.manager = this;
        }
    }

    public void OnPlayerPressStart()
    {
        // Remove button listener scripts
        foreach (var player in _gameStartInfo.players)
        {
            Destroy(player.GetComponent<PlayerButtonListener>());
        }
        
        // Go to main menu
        GameObject.FindWithTag("LoadingScreenDirector").GetComponent<LoadingScreenDirector>()
            .goTo = LoadingScreenDirector.GameScene.MainMenu;
        SceneManager.LoadScene("Loading");
    }
}
