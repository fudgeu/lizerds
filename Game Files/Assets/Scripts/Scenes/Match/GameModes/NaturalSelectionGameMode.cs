using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaturalSelectionGameMode : MonoBehaviour {
    private RoundLifecycleManager _roundLifecycleManager;
    
    void Start()
    {
        // TODO subscribe to the scene load/unload events instead
        _roundLifecycleManager = GameObject.FindWithTag("StageManager").GetComponent<RoundLifecycleManager>();

        _roundLifecycleManager.useRoundTimer = true;
        _roundLifecycleManager.roundTimer = 90;

        // TODO get all players in game, store em in a list, subscribe to their OnDeath events
    }

    void Update()
    {
        // TODO
        // if (alivePlayers === 1)
        //   _roundLifecycleManager.EndRound()
    }
}
