using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JoinMenu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameSetupSceneManager : MonoBehaviour
{
    [Header("Arenas")]
    public string[] arenas = new string[4]{ "Garden", "Jungle", "Desert", "Swamp" };
    public string[] gameModes = new string[2]{ "Natural Selection", "Predator" };
    
    [Header("References")]
    public GameObject radioButtonPrefab;
    public GameObject arenaButtonListObj;
    public GameObject gameModeButtonListObj;
    public EventSystem eventSystem;
    
    public Button startGameButton;
    
    // Internal
    private bool[] arenasChecked = new bool[4]{ true, true, true, true };
    private bool[] gameModesChecked = new bool[2]{ true, true };
    
    void Start()
    {
        // Create arena radio buttons
        AddRadioButtons(arenas, arenasChecked, arenaButtonListObj);
        AddRadioButtons(gameModes, gameModesChecked, gameModeButtonListObj);

        eventSystem.firstSelectedGameObject = arenaButtonListObj.transform.GetChild(1).gameObject;
        
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
        
        var filteredGameModes = new List<string>();
        for (int i = 0; i < gameModes.Length; i++)
        {
            if (gameModesChecked[i]) filteredGameModes.Add(gameModes[i]);
        }
        gameStartInfo.gameModes = filteredGameModes;

        SceneManager.LoadScene("PlayerJoinScene");
    }

    private void AddRadioButtons(string[] options, bool[] checkd, GameObject list)
    {
        for (int i = 0; i < options.Length; i++)
        {
            var option = options[i];
            var iCopy = i;
            
            var button = Instantiate(radioButtonPrefab, list.transform);
            var buttonController = button.GetComponent<RadioButtonController>();
            buttonController.isChecked = checkd[iCopy];
            
            buttonController.label = option;
            buttonController.onSelected = () =>
            {
                // Make sure we can't unselect the last one
                int totalChecked = 0;
                foreach (bool checkd in checkd) totalChecked += checkd ? 1 : 0;
                if (totalChecked == 1 && checkd[iCopy]) return;
                
                checkd[iCopy] = !checkd[iCopy];
                buttonController.isChecked = checkd[iCopy];
            };
        }
    }
}
