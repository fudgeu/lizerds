using System;
using UnityEngine;
using TMPro;
using Tweens;
using Unity.Mathematics;
using Unity.VisualScripting;
using Random = System.Random;

public class PlayerJoinSetup : MonoBehaviour
{
    private ProfileManager _profileManager;
    
    public TMP_Text playerLabel;
    
    private Random _random = new Random();
    
    private Rigidbody2D _rigidbody;

    void Start()
    {
        var profileInfo = GetComponentInParent<PlayerProfileInfo>();
        _profileManager = FindObjectOfType<ProfileManager>();
        
        // Set player number based on number of existing players
        int numPlayersInScene = GameObject.FindGameObjectsWithTag("Player").Length; // Number includes self
        profileInfo.playerNumber = numPlayersInScene;
        
        // Set default profile
        profileInfo.Profile = _profileManager.defaultProfile;
        profileInfo.CustomName = "Player " + profileInfo.playerNumber;
        profileInfo.UseCustomName = true;
        
        // Get internal components
        _rigidbody = GetComponent<Rigidbody2D>();
        
        // Shoot out in random direction on spawn
        _rigidbody.velocity = new Vector2((_random.Next(20) - 10) / 50f, (_random.Next(20) - 10) / 50f);
        
        // Exlpode scale effect on spawn
        gameObject.AddTween(new LocalScaleTween
            {
                from = new Vector3(0, 0, 0),
                to = new Vector3(1, 1, 1),
                duration = 0.3f,
                easeType = EaseType.BackOut,
            }
        );
        
        // Get game start manager
        var gameStartManager = GameObject.FindWithTag("GameStartManager").GetComponent<GameStartManager>();
        gameStartManager.registerPlayer(this.GameObject());
    }

    void Update()
    {
        Vector3 curPos = transform.position;
        
        // Add gravity towards center
        _rigidbody.AddForce(-curPos / 5);
        
        // Add very slight rotation
        // Vector3 rotationForce = Vector3.Normalize(Quaternion.Euler(0, 0, -135) * curPos);
        // _rigidbody.AddForce(rotationForce / 40);
    }
}
