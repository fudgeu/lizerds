using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script that spawns players into 
// the appropiate scene
// NOTE: written currently to load new scene on player death
// but new scenes have a preexisting player object
public class PlayerToScene : MonoBehaviour
{
    public static PlayerToScene Instance;

    //name array containing names for all stage types
    string[] stageName = { "Garden", "Jungle", "Desert", "Swamp"};

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        PlayerListener.GameCon += loadScene;
    }


    void OnDisable()
    {
        PlayerListener.GameCon -= loadScene;
    }



    void loadScene()
    {
        SceneManager.LoadScene(stageName[Random.Range(0, stageName.Length)]);
    }

    public void ChnageScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /*
     * initial stage load from game select
     * if game condition true
     *  load mutation scene
     * if mutation are done
     *  load different stage
     * 
     */
}
