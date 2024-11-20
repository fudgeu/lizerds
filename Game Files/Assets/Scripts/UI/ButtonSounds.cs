using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour
{
    public AudioClip onSelect;
    public AudioClip onClick;

    private Button _button;
    private AudioSource _audioSource;
    private EventSystem _es;
    private bool _selected = false;
    
    void OnEnable()
    {
        _button = GetComponent<Button>();
        _audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        _es = GetComponentInParent<PlayerInput>()?.uiInputModule?.gameObject?.GetComponent<EventSystem>();
        if (!_es)
        {
            _es = EventSystem.current;
        }
        
        _button.onClick.AddListener(HandleOnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleOnClick);
    }

    void Update()
    {
        if (!_selected && _es.currentSelectedGameObject == gameObject)
        {
            HandleOnSelect();
            _selected = true;
        }
        else if (_selected && _es.currentSelectedGameObject != gameObject)
        {
            _selected = false;
        }
    }

    private void HandleOnClick()
    {
        _audioSource.PlayOneShot(onClick);
    }

    private void HandleOnSelect()
    {
        _audioSource.PlayOneShot(onSelect);
    }
}
