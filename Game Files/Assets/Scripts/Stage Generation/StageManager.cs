using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    //
    string[] stageName = { "Garden", "Jungle"};

    // Start is called before the first frame update
    void Start()
    {
        loadScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void loadScene()
    {
        SceneManager.LoadScene(stageName[Random.Range(0, stageName.Length)]);
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
