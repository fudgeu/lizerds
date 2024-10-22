using System.Collections;
using System.Collections.Generic;
using JoinMenu;
using Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLifecycleManager : MonoBehaviour
{
    public int numRounds = 6;
    public int CurRound { get; private set; }
    public bool RoundInSession { get; private set; }
    
    // Events
    public delegate void RoundDelegate(int roundNumber,RoundLifecycleManager roundLifecycleManager);
    public delegate void Action();

    public event RoundDelegate OnRoundSetup; // Gets called when the RoundLifecycleManger is created and needs to be set up. Do not assume anything else exists yet.
    public event RoundDelegate OnRoundStart; // Gets called when the round is ready to start. Everything on the arena scene should exist at this point.
    public event Action OnRoundEnd; // Gets called when the round is over. Everything on the arena scene should still exist at this point.
    
    // Internal
    private GameStartInfo _gameStartInfo;
    private RoundLifecycleManager _roundLifecycleManager;
    
    void Awake()
    {
        print("Awaked");
        _gameStartInfo = GameObject.FindWithTag("GameStartInfo").GetComponent<GameStartInfo>();
        CurRound = 0;
        RoundInSession = false;
    }

    public void EndRound()
    {
        if (!RoundInSession) return;
        
        RoundInSession = false;
        OnRoundEnd?.Invoke();
        
        // Destroy all game players
        foreach (var player in _gameStartInfo.players)
        {
            var rootController = player.GetComponent<PlayerRootController>();
            Destroy(rootController.gamePlayerObject);
            rootController.gamePlayerObject = null;
        }
        
        // Destroy round lifecycle manager
        Destroy(_roundLifecycleManager.gameObject);

        // If this is the last round, go to results screen
        if (CurRound >= numRounds)
        {
            EndGame();
            return;
        }
        
        // Else, go to mutation phase
        SceneManager.LoadScene("Mutation");
    }
    
    public void StartRound()
    {
        // Increment round number
        CurRound++;
        print("Starting round number " + CurRound);
        
        // Check if we have exceeded the number of rounds. If so, kick to results screen.
        if (CurRound > numRounds)
        {
            EndGame();
            return;
        }
        
        // Create round lifecycle manager
        var roundManagerObj = new GameObject("RoundLifecycleManager");
        roundManagerObj.tag = "RoundLifecycleManager";
        DontDestroyOnLoad(roundManagerObj);
        _roundLifecycleManager = roundManagerObj.AddComponent<RoundLifecycleManager>();
        _roundLifecycleManager.gameLifecycleManager = this;
        
        // Set up game mode, if it doesn't exist
        // TODO check game settings object for gamemode, set up
        var gameModeObj = GameObject.FindWithTag("GameMode");
        if (gameModeObj is null)
        {
            gameModeObj = new GameObject("GameMode");
            gameModeObj.tag = "GameMode";
            DontDestroyOnLoad(gameModeObj);
            gameModeObj.AddComponent<NaturalSelectionGameMode>();
        }
        
        OnRoundSetup?.Invoke(CurRound, _roundLifecycleManager);
        
        // Set up loading screen to go to next arena
        var loadingScreenDirector = GameObject.FindWithTag("LoadingScreenDirector")?.GetComponent<LoadingScreenDirector>();
        if (loadingScreenDirector is null)
        {
            var directorGameObj = new GameObject("LoadingScreenDirector");
            directorGameObj.tag = "LoadingScreenDirector";
            DontDestroyOnLoad(directorGameObj);
            loadingScreenDirector = directorGameObj.AddComponent<LoadingScreenDirector>();
        }
        
        loadingScreenDirector.goTo = LoadingScreenDirector.GameScene.Arena;
        SceneManager.LoadScene("Loading");
    }

    public void OnRoundStarted()
    {
        RoundInSession = true;
        OnRoundStart?.Invoke(CurRound, _roundLifecycleManager);
    }

    public void EndGame()
    {
        // TODO
        RoundInSession = false;
        SceneManager.LoadScene("MainMenu");
    }
}
