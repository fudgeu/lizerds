using System.Collections;
using System.Collections.Generic;
using Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public string[] stageNames = { "Garden", "Jungle", "Desert", "Swamp" };

    void Start()
    {
        // Find out where we are trying to go
        var director = GameObject.FindWithTag("LoadingScreenDirector")?.GetComponent<LoadingScreenDirector>();
        if (director is null)
        {
            StartCoroutine(LoadMainMenu()); // Go to main menu if something went wrong
            return;
        }

        switch (director.goTo)
        {
            case LoadingScreenDirector.GameScene.MainMenu:
                StartCoroutine(LoadMainMenu());
                break;
            case LoadingScreenDirector.GameScene.MatchSetup:
                StartCoroutine(LoadMatchSetup());
                break;
            case LoadingScreenDirector.GameScene.Arena:
                StartCoroutine(LoadArena());
                break;
        }
    }

    private IEnumerator LoadMainMenu()
    {
        var sceneLoad = SceneManager.LoadSceneAsync("MainMenu");
        yield return null;
    }

    private IEnumerator LoadMatchSetup()
    {
        var sceneLoad = SceneManager.LoadSceneAsync("PlayerJoinScene");
        yield return null;
    }

    private IEnumerator LoadArena()
    {
        // See if there is a match settings object in scene, pick scene based off of that
        // TODO
        
        var sceneLoad = SceneManager.LoadSceneAsync(stageNames[Random.Range(0, stageNames.Length)]);
        yield return null;
    }
}
