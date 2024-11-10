using JoinMenu;
using Scenes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLifecycleManager : MonoBehaviour
{
    public int numRounds = 6;
    public int CurRound { get; private set; }
    public bool RoundInSession { get; private set; }

    [Header("Prefabs")]
    public GameObject scoreboardCanvasPrefab;
    public GameObject roundTimerPrefab;
    
    // Events
    public delegate void RoundDelegate(int roundNumber,RoundLifecycleManager roundLifecycleManager);
    public delegate void Action();

    public event RoundDelegate OnRoundSetup; // Gets called when the RoundLifecycleManger is created and needs to be set up. Do not assume anything else exists yet.
    public event RoundDelegate OnRoundStart; // Gets called when the round is ready to start. Everything on the arena scene should exist at this point.
    public event Action OnRoundEnd; // Gets called when the round is over. Everything on the arena scene should still exist at this point.
    
    // Internal
    private GameStartInfo _gameStartInfo;
    private RoundLifecycleManager _roundLifecycleManager;
    private LoadingScreenDirector _loadingScreenDirector;

    private List<Dictionary<GameObject, int>> _roundScoreboards = new();
    
    void Awake()
    {
        _gameStartInfo = GameObject.FindWithTag("GameStartInfo").GetComponent<GameStartInfo>();
        CurRound = 0;
        RoundInSession = false;
    }

    void Start()
    {
        _loadingScreenDirector = GameObject.FindWithTag("LoadingScreenDirector").GetComponent<LoadingScreenDirector>();
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
        
        // Grab scoreboard from round lifecycle manager and add it to tally
        var roundScoreboard = _roundLifecycleManager.GetPlayerScoreboard();
        _roundScoreboards.Add(roundScoreboard);
        
        // TODO remove
        // Find first place and print their name
        var sortedScores = roundScoreboard.OrderBy(entry => entry.Value);
        var firstPlace = sortedScores.Last();
        print("Player '" + firstPlace.Key.name + "' wins round " + CurRound + " with " + firstPlace.Value + " points!");
        
        // Destroy round lifecycle manager
        Destroy(_roundLifecycleManager.gameObject);

        // If this is the last round, go to results screen
        if (CurRound >= numRounds)
        {
            EndGame();
            return;
        }
        
        // Initiate scoreboard
        var scoreboardCanvas = Instantiate(scoreboardCanvasPrefab);
        var scoreboardSetup = scoreboardCanvas.GetComponent<EndRoundScoreboardSetup>();
        
        Dictionary<PlayerProfileInfo, int> scoreboardScores = new();
        foreach (var entry in roundScoreboard)
        {
            scoreboardScores.Add(entry.Key.GetComponent<PlayerProfileInfo>(), entry.Value);
        }
        scoreboardSetup.roundScoreboard = scoreboardScores;

        scoreboardSetup.ScoreboardFinished += () =>
        {
            SceneManager.LoadScene("Mutation");
        };
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
        _roundLifecycleManager.timerTextPrefab = roundTimerPrefab;
        
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
        // Print winner to console
        Dictionary<GameObject, int> totalScores = new();
        foreach (var roundScores in _roundScoreboards)
        {
            foreach (var scoreEntry in roundScores)
            {
                totalScores.TryAdd(scoreEntry.Key, 0);
                totalScores[scoreEntry.Key] += scoreEntry.Value;
            }
        }

        var sortedScores = totalScores.OrderBy(entry => entry.Value);
        var firstPlace = sortedScores.Last();
        print("Player '" + firstPlace.Key.name + "' wins the game with " + firstPlace.Value + " points!");
        
        // Go to results scoreboard screen
        Dictionary<PlayerProfileInfo, int> scoreboardScores = new();
        foreach (var entry in totalScores)
        {
            scoreboardScores.Add(entry.Key.GetComponent<PlayerProfileInfo>(), entry.Value);
        }

        var resultsInfoObj = new GameObject("ResultsInfo");
        DontDestroyOnLoad(resultsInfoObj);
        var resultsInfo = resultsInfoObj.AddComponent<ResultsInfo>();
        resultsInfo.gameScoreboard = scoreboardScores;
        
        // TODO
        RoundInSession = false;

        _loadingScreenDirector.goTo = LoadingScreenDirector.GameScene.Results;
        SceneManager.LoadScene("Loading");
    }
}
