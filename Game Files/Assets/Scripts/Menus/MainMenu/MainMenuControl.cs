using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    public Button startButton;
    public Button settingsButton;
    public Button quitButton;

    public GameObject settingsPanelPrefab;
    
    void Start()
    {
        // Add event listeners to buttons
        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameSetup");
        });

        settingsButton.onClick.AddListener(() =>
        {
            Instantiate(settingsPanelPrefab, transform);
        });
        
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
