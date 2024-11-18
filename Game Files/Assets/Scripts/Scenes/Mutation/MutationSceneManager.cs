using System.Collections.Generic;
using JoinMenu;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using Random = UnityEngine.Random;

public class MutationSceneManager : MonoBehaviour
{
    public delegate void TurnEndHandler(GameObject player, Mutation mutation);

    public GameObject mutationPlayerTurnCanvasPrefab;
    
    private GameStartInfo _gameStartInfo;
    private GameLifecycleManager _gameLifecycleManager;
    private List<GameObject> _sortedScoreboard;

    private MutationsManager _mutationsManager;
    private List<Mutation> _mutations = new();

    private GameObject _turnCanvas;
    private int _curPlayerTurn = 0;
    private List<GameObject> _usedPlayers = new();
    
    void Start()
    {
        _gameStartInfo = GameObject.FindWithTag("GameStartInfo").GetComponent<GameStartInfo>();
        _gameLifecycleManager = GameObject.FindWithTag("GameLifecycleManager").GetComponent<GameLifecycleManager>();
        _mutationsManager = FindObjectOfType<MutationsManager>();
        
        // Generate set of mutations
        for (int i = 0; i <= _gameStartInfo.players.Count; i++)
        {
            Mutation mutation;
            do
            {
                mutation = _mutationsManager.availableMutations[Random.Range(0, _mutationsManager.availableMutations.Length)];
            } while (_mutations.Contains(mutation));
            _mutations.Add(mutation);
        }
        
        // Get and sort round scoreboard
        var roundScoreboard = _gameLifecycleManager.roundScoreboards.Last();
        _sortedScoreboard = new();
        var s = roundScoreboard.OrderBy(pair => pair.Value);
        foreach (var pair in s) _sortedScoreboard.Add(pair.Key);
        
        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        // Grab player
        var curPlayerProfile = _sortedScoreboard[_curPlayerTurn].GetComponent<PlayerProfileInfo>();
        
        // Create instance of player turn canvas
        _turnCanvas = Instantiate(mutationPlayerTurnCanvasPrefab, _sortedScoreboard[_curPlayerTurn].transform);
        var turnController = _turnCanvas.GetComponentInChildren<MutationTurnController>();
        turnController.playerName = curPlayerProfile.UseCustomName ? curPlayerProfile.CustomName : curPlayerProfile.name;
        turnController.availableMutations = _mutations;
        turnController.usedPlayers = _usedPlayers;
        turnController.onTurnEnd = HandleTurnEnd;

        var multiplayerEventSystem = _sortedScoreboard[_curPlayerTurn].GetComponentInChildren<MultiplayerEventSystem>();
        var inputModule = _sortedScoreboard[_curPlayerTurn].GetComponentInChildren<InputSystemUIInputModule>();
        var playerInput = _sortedScoreboard[_curPlayerTurn].GetComponent<PlayerInput>();
        
        multiplayerEventSystem.playerRoot = _sortedScoreboard[_curPlayerTurn];
        playerInput.uiInputModule = inputModule;
    }

    private void HandleTurnEnd(GameObject player, Mutation mutation)
    {
        _mutationsManager.AssignSpecificMutation(player.GetComponentInChildren<PlayerMutations>(), mutation);
        _mutations.Remove(mutation);
        _usedPlayers.Add(player);
        
        Destroy(_turnCanvas);
        _curPlayerTurn++;

        if (_curPlayerTurn >= _gameStartInfo.players.Count)
        {
            // End mutation phase
            _gameLifecycleManager.StartRound();
        }
        else
        {
            StartPlayerTurn();
        }
    }
}
