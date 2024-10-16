using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.PlasticSCM.Editor.WebApi;

public class loadingScreen : MonoBehaviour
{
    public static loadingScreen instance;

    //name array containing names for all stage types
    string[] stageName = { "Garden", "Jungle", "Desert", "Swamp" };

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        PlayerListener.GameCon += LoadScene;
    }

    void OnDisable()
    {
        PlayerListener.GameCon -= LoadScene;
    }

    private void LoadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene != null)
        {
            int SceneInt = StageIsScene(currentScene);



            // if previous scene was the main menu
            // load a stage
            if (SceneInt == 0) 
            {
                print("LOADING STAGE");
               StartCoroutine(LoadStageScene());
            }

            // if previous scene was stage
            // load mutation
            if (SceneInt == 1)
            {
                StartCoroutine(LoadMutationScene());
            }

            // id previous scene was mutation
            // load stage
            if (SceneInt == 2)
            {
                StartCoroutine(LoadStageScene());
            }
        }
    }

    private int StageIsScene(Scene scene)
    {
        switch (scene.name)
        {
            case "Mutation":
                return 2;
            case "Garden":
                return 1;
            case "Jungle":
                return 1;
            case "Swamp":
                return 1;
            case "Desert":
                return 1;
            default:
                return 0;
        }
    }

    private IEnumerator LoadMutationScene()
    {
        AsyncOperation loadMutation = SceneManager.LoadSceneAsync("Mutation");

        yield return null;
    }

    private IEnumerator LoadStageScene()
    {
        AsyncOperation loadMutation = SceneManager.LoadSceneAsync(stageName[Random.Range(0, stageName.Length)]);

        yield return null;
    }

    private IEnumerator LoadEndScene()
    {
        AsyncOperation loadMutation = SceneManager.LoadSceneAsync("End");

        yield return null;
    }
}
