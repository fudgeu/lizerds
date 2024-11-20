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
    public RectTransform profilePanel;
    public RectTransform createProfilePanel;
    public GameObject profilePanelContent;

    // Texts
    public TMP_Text profileText;
    
    // Buttons
    public Button baseUiButton;
    public Button changeProfileButton;
    public Button defaultProfileButton;
    public Button newProfileButton;
    public Button firstKeyOnCreatePanel;
    public Button createProfileButton;
    public Button cancelCreateProfileButton;
    public List<Button> profileButtons;

    // Internal
    private PlayerInput _playerInput;
    private int _panelWidth = 370;
    private Page _curPage = Page.Main;
    private ProfileManager _profileManager;
    private GameStartManager _gameStartManager;
    private PlayerProfileInfo _profileInfo;
    private NewProfileController _newProfileController;
    
    void Start()
    {
        // Grab components
        _gameStartManager = GameObject.FindWithTag("GameStartManager").GetComponent<GameStartManager>();
        _playerInput = GetComponentInParent<PlayerInput>();
        _profileInfo = GetComponentInParent<PlayerProfileInfo>();
        _newProfileController = GetComponentInChildren<NewProfileController>();
        
        // Grab profile manager and create buttons
        _profileManager = FindObjectOfType<ProfileManager>();
        foreach (var profile in _profileManager.profiles)
        {
            Button profileButton = Instantiate(baseUiButton, mainPanel);
            profileButton.gameObject.AddComponent<ScrollChildController>();
            profileButton.transform.SetParent(profilePanelContent.transform, false);

            // Set button name
            profileButton.GetComponentInChildren<TMP_Text>().text = profile.name;
            
            // Register profile button click
            profileButton.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                _profileInfo.Profile = profile;
                _profileInfo.UseCustomName = false;
            });
            
            profileButtons.Add(profileButton);
        }
        
        // Register default profile button handler
        defaultProfileButton.onClick.AddListener(() =>
        {
            _profileInfo.CustomName = $"Player {_profileInfo.playerNumber}";
            _profileInfo.Profile = _profileManager.defaultProfile;
            _profileInfo.UseCustomName = true;
            SwitchToPage(Page.Main);
        });
        
        // Register on click handlers
        changeProfileButton.onClick.AddListener(() => SwitchToPage(Page.Profile));
        newProfileButton.onClick.AddListener(() =>
        {
            _newProfileController.profileName = "";
            _newProfileController.profileNameText.text = "";
            SwitchToPage(Page.CreateProfile);
        });
        cancelCreateProfileButton.onClick.AddListener(() => SwitchToPage(Page.Profile));
        createProfileButton.onClick.AddListener(() =>
        {
            Profile newProfile = new();
            newProfile.name = _newProfileController.profileName;
            _profileInfo.Profile = newProfile;
            _profileInfo.UseCustomName = false;
            _profileManager.profiles.Add(newProfile);
            SwitchToPage(Page.Main);
        });
        
        // Register player input handlers
        _playerInput.actions["Back"].performed += ctx =>
        {
            switch (_curPage)
            {
                case Page.Main:
                    // TODO: maybe play a 'bonk' sound effect that indicates they cant go further back?
                    break;
                case Page.Profile:
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
                if (_curPage == Page.Profile)
                {
                    UiUtil.DoRectTranslateAnim(mainPanel, new Vector2(_panelWidth, 0), duration, ease);
                    UiUtil.DoRectTranslateAnim(profilePanel, new Vector2(_panelWidth, 0), duration, ease);
                } else if (_curPage == Page.CreateProfile)
                {
                    UiUtil.DoRectTranslateAnim(mainPanel, new Vector2(_panelWidth, 0), duration, ease);
                    UiUtil.DoRectTranslateAnim(profilePanel, new Vector2(_panelWidth * 2, 0), duration, ease);
                    UiUtil.DoRectTranslateAnim(createProfilePanel, new Vector2(_panelWidth * 2, 0), duration, ease);
                }
                eventSystem.SetSelectedGameObject(changeProfileButton.gameObject);
                break;
            case Page.Profile:
                if (_curPage == Page.Main)
                {
                    UiUtil.DoRectTranslateAnim(mainPanel, new Vector2(-_panelWidth, 0), duration, ease);
                    UiUtil.DoRectTranslateAnim(profilePanel, new Vector2(-_panelWidth, 0), duration, ease);
                } else if (_curPage == Page.CreateProfile)
                {
                    UiUtil.DoRectTranslateAnim(profilePanel, new Vector2(_panelWidth, 0), duration, ease);
                    UiUtil.DoRectTranslateAnim(createProfilePanel, new Vector2(_panelWidth * 2, 0), duration, ease);
                }
                eventSystem.SetSelectedGameObject(defaultProfileButton.gameObject);
                break;
            case Page.CreateProfile:
                UiUtil.DoRectTranslateAnim(profilePanel, new Vector2(-_panelWidth, 0), duration, ease);
                UiUtil.DoRectTranslateAnim(createProfilePanel, new Vector2(-_panelWidth * 2, 0), duration, ease);
                eventSystem.SetSelectedGameObject(firstKeyOnCreatePanel.gameObject);
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
    
    enum Page { Main, Profile, CreateProfile }
}
