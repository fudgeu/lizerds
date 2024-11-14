using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewProfileController : MonoBehaviour
{
    public int keyTimeout = 1000;
    
    public TMP_Text profileNameText;
    
    public GameObject keyboardContainer;
    public Button shiftButton;
    public Button spaceButton;
    public Button backspaceButton;

    private Button[] keyboardButtons = new Button[9];

    private string[] keyboardKey = { "ABC", "DEF", "GHI", "JKL", "MNO", "PQRS", "TUV", "WXYZ", "!?$^" };

    public string profileName = "";
    private int curKeyIndex = -1;
    private int curKeySelection = -1;
    private int keyUpdateTimeout = 0;
    private bool shiftActivated = true;
    
    void Start()
    {
        // Get all keys from the keyboard
        for (int i = 0; i < 9; i++)
        {
            int iCopy = i;
            keyboardButtons[i] = keyboardContainer.transform.GetChild(i).gameObject.GetComponent<Button>();
            keyboardButtons[i].onClick.AddListener(() => OnKeyPress(iCopy));
        }
        
        // Register other keys
        spaceButton.onClick.AddListener(() =>
        {
            profileName += " ";
            profileNameText.text = profileName;
        });
        
        backspaceButton.onClick.AddListener(() =>
        {
            if (profileName.Length == 0) return;
            profileName = profileName.Remove(profileName.Length - 1);
            profileNameText.text = profileName;
        });
        
        shiftButton.onClick.AddListener(() =>
        {
            shiftActivated = !shiftActivated;
        });
    }

    void FixedUpdate()
    {
        if (keyUpdateTimeout > 0)
        {
            keyUpdateTimeout--;
            if (keyUpdateTimeout <= 0) shiftActivated = false;
        }
    }

    private void OnKeyPress(int keyIndex)
    {
        if (keyUpdateTimeout <= 0 || keyIndex != curKeyIndex)
        {
            // Key is freshly pressed
            if (keyUpdateTimeout > 0) shiftActivated = false;
            keyUpdateTimeout = keyTimeout;
            curKeyIndex = keyIndex;
            curKeySelection = 0;
            profileName += shiftActivated ? keyboardKey[keyIndex][0] : keyboardKey[keyIndex][0].ToString().ToLower();
        }
        else
        {
            // Key is re-pressed
            profileName = profileName.Remove(profileName.Length - 1);
            keyUpdateTimeout = keyTimeout;
            curKeySelection = (curKeySelection + 1) % (keyboardKey[keyIndex].Length);
            profileName += shiftActivated ? keyboardKey[keyIndex][curKeySelection] : keyboardKey[keyIndex][curKeySelection].ToString().ToLower();
        }
        
        profileNameText.text = profileName;
    }
}
