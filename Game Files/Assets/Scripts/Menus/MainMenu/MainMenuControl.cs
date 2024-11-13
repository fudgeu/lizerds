using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    
    void Start()
    {
        // Add event listeners to buttons
        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameSetup");
        });
        
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
