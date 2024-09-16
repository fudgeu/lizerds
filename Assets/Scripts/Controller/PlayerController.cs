using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //--Components--------------------
    [Header("Components")]
    [Tooltip("The Player Input component attached to this object")]
        [SerializeField] private PlayerInput inputAsset;

    private InputActionMap playerInputActionMap;

    [Tooltip("The rigidbody attached to this object")]
        [SerializeField] private new Rigidbody2D rigidbody;

    //--Input Actions-----------------
    private InputAction move;
    private InputAction leap;

    //--Movement Configuration--------
    [Header("Movement Configuration")]
    [Tooltip("The player's maximum movement speed")]
        public float moveSpeed;                         //<-- This is public because different evolutions will likely alter it
    [Tooltip("The rate the player accelerates at.")]
        [SerializeField] private float acceleration;
    [Tooltip("The rate the player deccelerates at.")]
        [SerializeField] private float decceleration;
    [Tooltip("Controls the RESPONSIVENESS of new directional input. Not entirely sure how it works, but effects are dramatic")]
        [SerializeField] private float velPower;

    [Tooltip("The force applied in the target direction when leap is used")]
        public Vector2 leapForce;


    // Awake: Runs before Start() and OnEnable() (very first thing)
    private void Awake()
    {
        //playerInputActions = new PlayerInputActions();
        playerInputActionMap = inputAsset.currentActionMap;
    }

    //OnEnable: Runs when player prefab spawns
    private void OnEnable()
    {
        //Link and enable inputactions
        //Movement
        //move = playerInputActions.Player.Move;
        move = playerInputActionMap.FindAction("Move");
        move.Enable();

        //Leap
        leap = playerInputActionMap.FindAction("Leap");
        leap.performed += DoLeap;
        leap.Enable();

        //Light Attack
            //to do...
        //Heavy Attack
            //to do...
        //Grab
            //to do...
    }

    //OnDisable: Unlink inputs when object is disabled (prevents them from being persistent)
    private void OnDisable()
    {
        move.Disable();
        leap.Disable();
    }

    //DoLeap: Send player in their look direction + small upward movemennt (unless they are trying to leap down)
    private void DoLeap(InputAction.CallbackContext context)
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.AddForce(leapForce * move.ReadValue<Vector2>() + new Vector2(0 , 5), ForceMode2D.Impulse);
        Debug.Log("LEAPING!");
    }

    private void FixedUpdate()
    {
        #region Run
        float targetSpeed = move.ReadValue<Vector2>().x * moveSpeed;

        float speedDif = targetSpeed - rigidbody.velocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        rigidbody.AddForce(movement * Vector2.right);

        //Debug.Log("Input: " + move.ReadValue<Vector2>());
        #endregion
    }

}
