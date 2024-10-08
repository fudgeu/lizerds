using System;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
using Random = System.Random;

public class UiSetup : MonoBehaviour
{
    public int playerNumber = 0;
    public TMP_Text playerLabel;
    
    private Random _random = new Random();
    
    private Rigidbody2D _rigidbody;

    void Start()
    {
        // Set player number based on number of existing players
        int numPlayersInScene = GameObject.FindGameObjectsWithTag("Player").Length; // Number includes self
        playerNumber = numPlayersInScene;
        
        // Update label
        playerLabel.text = "Player " + playerNumber;
        
        // Get internal components
        _rigidbody = GetComponent<Rigidbody2D>();
        
        // Shoot out in random direction on spawn
        _rigidbody.velocity = new Vector2(_random.Next(10) / 50f, _random.Next(10) / 50f);
    }

    void Update()
    {
        Vector3 curPos = transform.position;
        
        // Add gravity towards center
        _rigidbody.AddForce(-curPos / 10);
        
        // Add very slight rotation
        Vector3 rotationForce = Vector3.Normalize(Quaternion.Euler(0, 0, -135) * curPos);
        _rigidbody.AddForce(rotationForce / 20);
    }
}
