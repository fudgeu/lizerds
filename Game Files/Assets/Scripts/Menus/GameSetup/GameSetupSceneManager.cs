using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JoinMenu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetupSceneManager : MonoBehaviour
{
    [Header("Arenas")]
    public string[] arenas = new string[4]{ "Garden", "Jungle", "Desert", "Swamp" };
    
    [Header("References")]
    public GameObject radioButtonPrefab;
    public GameObject buttonListObj;
    public EventSystem eventSystem;
    
    public Button startGameButton;
    
    // Internal
    private bool[] arenasChecked = new bool[4]{ false, false, false, false };
    
    void Start()
    {
        // Create arena radio buttons
        for (int i = 0; i < arenas.Length; i++)
        {
            var arena = arenas[i];
            var iCopy = i;
            
            var button = Instantiate(radioButtonPrefab, buttonListObj.transform);
            var buttonController = button.GetComponent<RadioButtonController>();
            buttonController.isChecked = arenasChecked[iCopy];
            
            buttonController.label = arena;
            buttonController.onSelected = () =>
            {
                // Make sure we can't unselect the last one
                int totalChecked = 0;
                foreach (bool checkd in arenasChecked) totalChecked += checkd ? 1 : 0;
                if (totalChecked == 1 && arenasChecked[iCopy]) return;
                
                arenasChecked[iCopy] = !arenasChecked[iCopy];
                buttonController.isChecked = arenasChecked[iCopy];
            };

            if (i == 0)
            {
                eventSystem.firstSelectedGameObject = button.gameObject;
            }
        }
        
        // Register start game button handler
        startGameButton.onClick.AddListener(HandleStartGame);
    }

    private void OnDestroy()
    {
        startGameButton.onClick.RemoveAllListeners();
    }

    private void HandleStartGame()
    {
        // Create game info object that will contain game settings
        GameObject gameStartInfoObj = GameObject.FindWithTag("GameStartInfo");
        if (gameStartInfoObj is null)
        {
            gameStartInfoObj = new GameObject("GameStartInfo");
            gameStartInfoObj.AddComponent<GameStartInfo>();
            DontDestroyOnLoad(gameStartInfoObj);
            gameStartInfoObj.tag = "GameStartInfo";
        }
        var gameStartInfo = gameStartInfoObj.GetComponent<GameStartInfo>();
        
        var filteredArenas = new List<string>();
        for (int i = 0; i < arenas.Length; i++)
        {
            if (arenasChecked[i]) filteredArenas.Add(arenas[i]);
        }
        
        gameStartInfo.arenas = filteredArenas;

        SceneManager.LoadScene("PlayerJoinScene");
    }
}
