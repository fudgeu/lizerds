using System;
using System.Collections;
using System.Collections.Generic;
using JoinMenu;
using Tweens;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageStartManager : MonoBehaviour
{
    public GameObject gamePlayerPrefab;
    public GameObject stageStartScreenPrefab;
    
    private Camera _camera;
    
    void Start()
    {
        // Get lifecycle manager
        var gameLifecycleManager = GameObject.FindWithTag("GameLifecycleManager").GetComponent<GameLifecycleManager>();
        
        // Get players
        var players = GameObject.FindGameObjectWithTag("GameStartInfo").GetComponent<GameStartInfo>().players;
        
        // Spawn players
        var spawnPoints = GameObject.FindGameObjectsWithTag("Respawn");
        var usedSpawnPoints = new HashSet<GameObject>();

        int i = 1;
        foreach (var player in players)
        {
            // Create game player obj and set parent
            var gamePlayer = Instantiate(gamePlayerPrefab, player.transform);
            gamePlayer.layer = LayerMask.NameToLayer($"P{i}");
            
            // Place at random spawn point
            if (spawnPoints.Length != 0)
            {
                GameObject spawnPoint = null;
                do
                {
                    spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                } while (usedSpawnPoints.Contains(spawnPoint));
                usedSpawnPoints.Add(spawnPoint);
                gamePlayer.transform.position = spawnPoint.transform.position;   
            }
            
            // Set up root properties
            player.GetComponent<PlayerRootController>().gamePlayerObject = gamePlayer;
            
            // Enable all mutations
            player.GetComponent<PlayerMutations>().EnableAllMutations();

            i = Math.Min(4, i + 1);
        }
        
        // Create the stage start screen
        Instantiate(stageStartScreenPrefab);
        
        // Get and animation camera
        _camera = FindObjectOfType<Camera>();
        _camera.orthographicSize = 10f;
        var cameraTween = new FloatTween
        {
            from = 10f,
            to = 5f,
            duration = 2,
            onUpdate = (_, newVal) => _camera.orthographicSize = newVal,
            easeType = EaseType.QuintInOut,
            delay = 2,
            onEnd = (_) => gameLifecycleManager.OnRoundStarted(),

        };
        gameObject.AddTween(cameraTween);

        var bkg = GameObject.FindGameObjectWithTag("Background");
        float initialScale = bkg.transform.localScale.x;
        float start = (initialScale + 0.5f) * 1.5f;
        bkg.transform.localScale = new Vector3(start, start, start);
        var bkgTween = new FloatTween
        {
            from = start,
            to = initialScale + 0.5f,
            duration = 2,
            onUpdate = (_, newVal) => bkg.transform.localScale = new Vector3(newVal, newVal, newVal),
            easeType = EaseType.QuintInOut,
            delay = 2,
        };
        gameObject.AddTween(bkgTween);
    }
}
