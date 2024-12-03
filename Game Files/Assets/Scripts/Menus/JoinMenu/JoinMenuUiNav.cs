using System;
using System.Collections.Generic;
using TMPro;
using Tweens;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class JoinMenuUiNav : MonoBehaviour
{
    // Inputs
    public EventSystem eventSystem;
    
    // Panels
    public RectTransform mainPanel;
    public RectTransform createProfilePanel;
    public RectTransform appearancePanel;
    
    // Texts
    public TMP_Text profileText;
    
    // Buttons
    public Button changeProfileButton;
    public Button firstKeyOnCreatePanel;
    public Button createProfileButton;
    public Button cancelCreateProfileButton;
    public Button appearanceButton;
    public Button bodyColorButton;
    public Button jawColorButton;
    public Button eyeColorButton;

    // Internal
    private PlayerInput _playerInput;
    private int _panelWidth = 370;
    private Page _curPage = Page.Main;
    private ProfileManager _profileManager;
    private GameStartManager _gameStartManager;
    private PlayerProfileInfo _profileInfo;
    private NewProfileController _newProfileController;
    
    // Colors
    public List<String> colorNames = new();
    public List<Color> bodyColors = new();
    public List<Color> jawColors = new();
    private int _bodyColorIndex;
    private int _jawColorIndex;
    
    public List<String> eyeColorNames = new();
    public List<Color> eyeColors = new();
    private int _eyeColorIndex = 0;
    
    void Start()
    {
        // Grab components
        _gameStartManager = GameObject.FindWithTag("GameStartManager").GetComponent<GameStartManager>();
        _playerInput = GetComponentInParent<PlayerInput>();
        _profileInfo = GetComponentInParent<PlayerProfileInfo>();
        _newProfileController = GetComponentInChildren<NewProfileController>();
        _profileManager = FindObjectOfType<ProfileManager>();
        
        // Set default colors
        _bodyColorIndex = _profileInfo.playerNumber - 1;
        _jawColorIndex = _profileInfo.playerNumber - 1;
        _profileInfo.bodyColor = bodyColors[_bodyColorIndex];
        _profileInfo.jawColor = jawColors[_jawColorIndex];
        _profileInfo.eyeColor = eyeColors[_eyeColorIndex];
        UpdateAppearanceTexts();

        // Register on click handlers
        changeProfileButton.onClick.AddListener(() => SwitchToPage(Page.CreateProfile));
        cancelCreateProfileButton.onClick.AddListener(() => SwitchToPage(Page.Main));
        createProfileButton.onClick.AddListener(() =>
        {
            Profile newProfile = new();
            newProfile.name = _newProfileController.profileName;
            _profileInfo.Profile = newProfile;
            _profileInfo.UseCustomName = false;
            _profileManager.profiles.Add(newProfile);
            SwitchToPage(Page.Main);
        });
        
        appearanceButton.onClick.AddListener(() => SwitchToPage(Page.Appearance));

        bodyColorButton.onClick.AddListener(() =>
        {
            _bodyColorIndex = (_bodyColorIndex + 1) % bodyColors.Count;
            _profileInfo.bodyColor = bodyColors[_bodyColorIndex];
            UpdateAppearanceTexts();
        });
        jawColorButton.onClick.AddListener(() =>
        {
            _jawColorIndex = (_jawColorIndex + 1) % jawColors.Count;
            _profileInfo.jawColor = jawColors[_jawColorIndex];
            UpdateAppearanceTexts();
        });
        eyeColorButton.onClick.AddListener(() =>
        {
            _eyeColorIndex = (_eyeColorIndex + 1) % eyeColors.Count;
            _profileInfo.eyeColor = eyeColors[_eyeColorIndex];
            UpdateAppearanceTexts();
        });
        
        // Register player input handlers
        _playerInput.actions["Back"].performed += ctx =>
        {
            switch (_curPage)
            {
                case Page.Main:
                    // TODO: maybe play a 'bonk' sound effect that indicates they cant go further back?
                    break;
                case Page.CreateProfile:
                case Page.Appearance:
                    SwitchToPage(Page.Main);
                    break;
            }
        };
        
        // Register start button handlers
        _playerInput.actions["Join"].started += HandleOnPressStart;

        _playerInput.actions["Join"].canceled += HandleOnCancelStart;
        
        // Register profile text changer
        _profileInfo.OnProfileChanged += handleProfileChange;
        
        // Run one time for initial set up
        handleProfileChange();
    }

    private void OnDestroy()
    {
        _playerInput.actions["Join"].started -= HandleOnPressStart;
        _playerInput.actions["Join"].canceled -= HandleOnCancelStart;
    }

    private void SwitchToPage(Page page)
    {
        // Animation settings
        var ease = EaseType.BackOut;
        var duration = 0.2f;
        
        switch (page)
        {
            case Page.Main:
                // if (_curPage == Page.Profile)
                // {
                //     UiUtil.DoRectTranslateAnim(mainPanel, new Vector2(_panelWidth, 0), duration, ease);
                //     UiUtil.DoRectTranslateAnim(profilePanel, new Vector2(_panelWidth, 0), duration, ease);
                // } else if (_curPage == Page.CreateProfile)
                // {
                //     UiUtil.DoRectTranslateAnim(mainPanel, new Vector2(_panelWidth, 0), duration, ease);
                //     UiUtil.DoRectTranslateAnim(profilePanel, new Vector2(_panelWidth * 2, 0), duration, ease);
                //     UiUtil.DoRectTranslateAnim(createProfilePanel, new Vector2(_panelWidth * 2, 0), duration, ease);
                // }
                if (_curPage == Page.CreateProfile)
                {
                    UiUtil.DoRectTranslateAnim(mainPanel, new Vector2(_panelWidth, 0), duration, ease);
                    UiUtil.DoRectTranslateAnim(createProfilePanel, new Vector2(_panelWidth, 0), duration, ease);
                }
                else if (_curPage == Page.Appearance)
                {
                    UiUtil.DoRectTranslateAnim(mainPanel, new Vector2(_panelWidth, 0), duration, ease);
                    UiUtil.DoRectTranslateAnim(appearancePanel, new Vector2(_panelWidth, 0), duration, ease);
                }
                eventSystem.SetSelectedGameObject(changeProfileButton.gameObject);
                break;
            case Page.CreateProfile:
                UiUtil.DoRectTranslateAnim(mainPanel, new Vector2(-_panelWidth, 0), duration, ease);
                UiUtil.DoRectTranslateAnim(createProfilePanel, new Vector2(-_panelWidth, 0), duration, ease);
                eventSystem.SetSelectedGameObject(firstKeyOnCreatePanel.gameObject);
                break;
            case Page.Appearance:
                UiUtil.DoRectTranslateAnim(mainPanel, new Vector2(-_panelWidth, 0), duration, ease);
                UiUtil.DoRectTranslateAnim(appearancePanel, new Vector2(-_panelWidth, 0), duration, ease);
                eventSystem.SetSelectedGameObject(bodyColorButton.gameObject);
                break;
        }
        
        _curPage = page;
    }

    private void handleProfileChange()
    {
        if (_profileInfo.UseCustomName)
        {
            profileText.text = _profileInfo.CustomName ?? "Unknown";
        }
        else
        {
            profileText.text = _profileInfo.Profile.name;
        }
    }

    private void HandleOnPressStart(InputAction.CallbackContext ctx)
    {
        _gameStartManager.onPlayerHoldStart(gameObject);
    }

    private void HandleOnCancelStart(InputAction.CallbackContext ctx)
    {
        _gameStartManager.onPlayerLetGoStart(gameObject);
    }

    private void UpdateAppearanceTexts()
    {
        bodyColorButton.GetComponentInChildren<TMP_Text>().text = $"Body: {colorNames[_bodyColorIndex]}";
        jawColorButton.GetComponentInChildren<TMP_Text>().text = $"Jaw: {colorNames[_jawColorIndex]}";
        eyeColorButton.GetComponentInChildren<TMP_Text>().text = $"Eyes: {eyeColorNames[_eyeColorIndex]}";
    }
    
    enum Page { Main, CreateProfile, Appearance }
}
