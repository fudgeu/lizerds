using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    public GameObject holdToStartText;

    private List<GameObject> _registeredPlayers;

    public void registerPlayer(GameObject player)
    {
        _registeredPlayers.Add(player);
        onPlayerChangeOccured();
    }

    public void unregisterPlayer(GameObject player)
    {
        _registeredPlayers.Remove(player);
        onPlayerChangeOccured();
    }

    private void onPlayerChangeOccured()
    {
        print(_registeredPlayers.Count);
        if (_registeredPlayers.Count > 1)
        {
            // Enable starting
            holdToStartText.SetActive(true);
        }
        else
        {
            // Disable starting
            holdToStartText.SetActive(false);
        }
    }
}
