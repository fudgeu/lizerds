using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] GameObject player;

    void OnEnable()
    {
        PlayerListener.GameCon += respawn;
    }

    void OnDisable()
    {
        PlayerListener.GameCon -= respawn;
    }

    void respawn()
    {
        gameObject.transform.position = Vector2.zero;
    }

}
