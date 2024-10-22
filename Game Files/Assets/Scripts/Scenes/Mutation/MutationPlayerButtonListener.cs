using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MutationPlayerButtonListener : MonoBehaviour
{
    public MutationSceneManager mutationSceneManager;
    private PlayerInput _playerInput;

    void Start()
    {
        // Get player input
        _playerInput = GetComponent<PlayerInput>();
        
        // Register button listeners
        _playerInput.actions["Join"].performed += OnStartPressed;
    }

    void OnDestroy()
    {
        _playerInput.actions["Join"].performed -= OnStartPressed;
    }

    private void OnStartPressed(InputAction.CallbackContext ctx)
    {
        mutationSceneManager.OnPlayerPressStart();
    }
}
