using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameUI : MonoBehaviour
{
    public MovementTargetController movementTargetController;
    public Transform root;
    
    void Start()
    {
        var profileInfo = GetComponentInParent<PlayerProfileInfo>();
        var text = GetComponent<TMP_Text>();

        text.text = profileInfo.UseCustomName ? profileInfo.CustomName : profileInfo.Profile.name;
    }

    void Update()
    {
        if (root.localScale.x.Equals(-1))
        {
            transform.parent.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.parent.localScale = new Vector3(1, 1, 1);
        }
        transform.parent.rotation = Quaternion.Euler(0, 0, 0);
    }
}
