using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    public TMP_Text playerNameText;
    public TMP_Text healthText;
    public GameObject player;
    
    void Start()
    {
        var profile = player.GetComponent<PlayerProfileInfo>();
        playerNameText.text = profile.UseCustomName ? profile.CustomName : profile.name;

        player.GetComponentInChildren<PlayerHealth>().OnHealthChanged += (newHealth) =>
        {
            healthText.text = $"{newHealth}%";
        };
    }

    
}
