using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfileInfo : MonoBehaviour
{
    private Profile _profile;
    public Profile Profile
    {
        get => _profile;
        set
        {
            _profile = value;
            OnProfileChanged?.Invoke();
        }
    }

    private bool _useCustomName = false;
    public bool UseCustomName
    {
        get => _useCustomName;
        set
        {
            _useCustomName = value;
            OnProfileChanged?.Invoke();
        }
    }

    private string _customName;
    public string CustomName
    {
        get => _customName;
        set
        {
            _customName = value;
            OnProfileChanged?.Invoke();
        }
    }

    public int playerNumber;

    public delegate void ProfileChangedDelegate();
    public event ProfileChangedDelegate OnProfileChanged;

    public Color bodyColor;
    public Color jawColor;
    public Color eyeColor;
}
