using JoinMenu;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scenes;
using UnityEngine;
using UnityEngine.InputSystem;
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
            case LoadingScreenDirector.GameScene.Results:
                StartCoroutine(LoadResults());
                break;
        }
    }

    private IEnumerator LoadMainMenu()
    {
        Destroy(FindObjectOfType<PlayerInputManager>().gameObject);
        Destroy(FindObjectOfType<GameStartInfo>().gameObject);
        Destroy(FindObjectOfType<GameLifecycleManager>().gameObject);
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            Destroy(player);
        }
        
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
        var gameInfoObj = GameObject.FindGameObjectWithTag("GameStartInfo");
        var gameInfo = gameInfoObj?.GetComponent<GameStartInfo>();
        
        var arenas = gameInfo?.arenas ?? stageNames.ToList();
        
        var sceneLoad = SceneManager.LoadSceneAsync(arenas[Random.Range(0, arenas.Count)]);
        yield return null;
    }

    private IEnumerator LoadResults()
    {
        var sceneLoad = SceneManager.LoadSceneAsync("Results");
        yield return null;
    }
}
