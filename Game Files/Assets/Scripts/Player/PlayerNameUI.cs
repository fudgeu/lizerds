using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameUI : MonoBehaviour
{
    void Start()
    {
        var profileInfo = GetComponentInParent<PlayerProfileInfo>();
        var text = GetComponent<TMP_Text>();

        text.text = profileInfo.UseCustomName ? profileInfo.CustomName : profileInfo.Profile.name;
    }
}
