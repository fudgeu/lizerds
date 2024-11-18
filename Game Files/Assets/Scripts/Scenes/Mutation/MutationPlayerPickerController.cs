using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MutationPlayerPickerController : MonoBehaviour
{
    public delegate void HandleSelectPlayer(GameObject player);
    public delegate void HandleOnReturn();
    
    [Header("Props")]
    public HandleSelectPlayer onSelectPlayer;
    public HandleOnReturn onReturn;
    public List<GameObject> players;
    public List<GameObject> disabledPlayers;
    public Mutation mutation;
    

    [Header("References")]
    public GameObject card;
    public GameObject list;

    [Header("Prefabs")]
    public GameObject uiButtonPrefab;

    private PlayerInput _playerInput;
    
    void Start()
    {
        // Setup card
        card.GetComponent<MutationCardController>().mutationLabel = mutation.name;
        
        // Create button for each player
        var curPlayer = GetComponentInParent<PlayerProfileInfo>();
        var curPlayerButton = CreatePlayerButton(curPlayer.gameObject);
        curPlayerButton.GetComponentInChildren<TMP_Text>().text = "Yourself";
        players.Remove(curPlayer.gameObject);
        
        FindObjectOfType<EventSystem>().SetSelectedGameObject(curPlayerButton.gameObject);
        
        foreach (var player in players)
        {
            CreatePlayerButton(player);
        }
        
        // Attach B button listener to return
        _playerInput = curPlayer.gameObject.GetComponent<PlayerInput>();
        _playerInput.actions["Back"].performed += HandlePressBack;
    }

    private Button CreatePlayerButton(GameObject player)
    {
        var profile = player.GetComponent<PlayerProfileInfo>();
        var buttonObj = Instantiate(uiButtonPrefab, list.transform);
        var button = buttonObj.GetComponent<Button>();
        button.onClick.AddListener(() => onSelectPlayer?.Invoke(player));
        button.GetComponentInChildren<TMP_Text>().text = profile.UseCustomName ? profile.CustomName : profile.name;
        if (disabledPlayers.Contains(player))
        {
            button.interactable = false;
        }

        return button;
    }

    private void HandlePressBack(InputAction.CallbackContext ctx)
    {
        _playerInput.actions["Back"].performed -= HandlePressBack;
        onReturn?.Invoke();
    }
}
