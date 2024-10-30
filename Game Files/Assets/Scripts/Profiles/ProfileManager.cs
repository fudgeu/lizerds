using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public List<Profile> profiles = new();
    public Profile defaultProfile;
    
    void Start()
    {
        // Load profiles from file
    }
}
