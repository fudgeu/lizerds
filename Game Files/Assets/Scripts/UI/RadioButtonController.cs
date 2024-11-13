using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RadioButtonController : MonoBehaviour
{
    // Props
    public bool isChecked = false;
    public string label = "Button";
    public Action onSelected;

    // References
    public GameObject radioObject;
    
    // Internal
    private bool _lastCheckedState = false;
    private Button _button;
    private TMP_Text _label;
    
    void Start()
    {
        _button = GetComponent<Button>();
        _label = GetComponentInChildren<TMP_Text>();
        
        _label.text = label;
        _button.onClick.AddListener(() => onSelected());
    }

    void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        if (_lastCheckedState == isChecked) return;
        
        _lastCheckedState = isChecked;

        radioObject.SetActive(isChecked);
    }
}
