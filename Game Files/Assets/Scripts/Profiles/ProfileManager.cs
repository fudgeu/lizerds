using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public List<Profile> profiles = new();
    public Profile defaultProfile = new Profile { name = "Default" };
    
    void Start()
    {
        // Load profiles from file
    }
}
