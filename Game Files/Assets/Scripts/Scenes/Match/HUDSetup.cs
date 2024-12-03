using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDSetup : MonoBehaviour
{
    public List<GameObject> players;
    public GameObject playerHealthsPanel;
    public GameObject playerHealthPrefab;
    
    void Start()
    {
        foreach (var player in players)
        {
            var playerHealthUi = Instantiate(playerHealthPrefab, playerHealthsPanel.transform);
            var playerHealthController = playerHealthUi.GetComponent<PlayerHealthUI>();
            playerHealthController.player = player;
        }
    }
}
