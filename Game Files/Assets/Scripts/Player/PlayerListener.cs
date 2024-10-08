using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

// Script that trigger events
// based on player actions
public class PlayerListener : MonoBehaviour
{
    public static event Action GameCon;

    private bool gameEnded;
    public void WinLevel()
    {
        if (!gameEnded)
        {
            Debug.Log("WIN");
            gameEnded = true;
            if (GameCon != null)
                GameCon();
        }
    }

    public void LoseLevel()
    {
        if (!gameEnded)
        {
            Debug.Log("LOSE");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            gameEnded = true;
        }
    }
}
