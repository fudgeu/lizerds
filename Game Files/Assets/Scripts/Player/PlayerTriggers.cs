using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Player script that calls
// functions which trigger events
// based on player actions
public class PlayerTriggers : MonoBehaviour
{
    public PlayerListener PlayerListener;
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -5.0f)
        {
            PlayerListener.WinLevel();
        }
    }
}
