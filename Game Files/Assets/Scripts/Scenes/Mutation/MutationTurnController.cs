using JoinMenu;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MutationTurnController : MonoBehaviour
{
    // To be injected by the scene manager
    public string playerName;
    public List<GameObject> usedPlayers;
    public List<Mutation> availableMutations;
    public MutationSceneManager.TurnEndHandler onTurnEnd;
    
    // Prefabs
    public GameObject mutationCardPrefab;
    public GameObject mutationCardsContainerPrefab;
    public GameObject mutationPlayerPickerPrefab;
    
    // References
    public TMP_Text playerTitleText;
    public GameObject mainContainer;
    
    private GameObject _mutationCardsContainer;
    private GameObject _playerPickerContainer;


    void Start()
    {
        // Change player title text
        playerTitleText.text = $"{playerName}'s Turn";
        SetupMutationCards();
    }

    private void SetupMutationCards()
    {
        // Destroy player picker, if present
        if (_playerPickerContainer)
            Destroy(_playerPickerContainer);
        
        _mutationCardsContainer = Instantiate(mutationCardsContainerPrefab, mainContainer.transform);
        
        // Add mutation cards for each available mutation
        int i = 0;
        foreach (var mutation in availableMutations)
        {
            var mutationCard = Instantiate(mutationCardPrefab, _mutationCardsContainer.transform);
            var mutationCardController = mutationCard.GetComponent<MutationCardController>();
            mutationCardController.mutationLabel = mutation.mutationName;
            mutationCardController.button.onClick.AddListener(() => OnPressMutation(mutation));

            if (i == 0)
            {
                var eventSystem = FindObjectOfType<EventSystem>();
                eventSystem.SetSelectedGameObject(mutationCard);
            }

            i++;
        }
    }

    private void OnPressMutation(Mutation mutation)
    {
        // Delete mutation cards
        Destroy(_mutationCardsContainer);
        
        // Insert person picker
        _playerPickerContainer = Instantiate(mutationPlayerPickerPrefab, mainContainer.transform);
        var playerPickerController = _playerPickerContainer.GetComponent<MutationPlayerPickerController>();
        playerPickerController.players = new List<GameObject>(FindObjectOfType<GameStartInfo>().players);
        playerPickerController.disabledPlayers = usedPlayers;
        playerPickerController.mutation = mutation;
        playerPickerController.onReturn = SetupMutationCards;
        playerPickerController.onSelectPlayer = (player) => OnApplyMutationToPlayer(player, mutation);
    }

    private void OnApplyMutationToPlayer(GameObject player, Mutation mutation)
    {
        onTurnEnd(player, mutation);
    }
}
