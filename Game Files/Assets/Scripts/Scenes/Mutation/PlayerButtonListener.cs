using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerButtonListener : MonoBehaviour
{
    public ButtonManager manager;
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
        manager.OnPlayerPressStart();
    }
}
