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
    public GameObject profilePanelContent;

    // Buttons
    public Button baseUiButton;
    public Button changeProfileButton;
    public Button defaultProfileButton;
    public List<Button> profileButtons;

    // Internal
    private PlayerInput _playerInput;
    private int _panelWidth = 250;
    private Page _curPage = Page.Main;
    private ProfileManager _profileManager;
    private GameStartManager _gameStartManager;
    
    void Start()
    {
        // Grab components
        _gameStartManager = GameObject.FindWithTag("GameStartManager").GetComponent<GameStartManager>();
        _playerInput = GetComponentInParent<PlayerInput>();
        
        // Grab profile manager and create buttons
        _profileManager = FindObjectOfType<ProfileManager>();
        foreach (var profile in _profileManager.profiles)
        {
            Button profileButton = Instantiate(baseUiButton, mainPanel);
            profileButton.GetComponentInChildren<TMP_Text>().text = profile.name;
            profileButton.transform.SetParent(profilePanelContent.transform, false);
            profileButtons.Add(profileButton);
        }
        
        // Register on click handlers
        changeProfileButton.onClick.AddListener(() => SwitchToPage(Page.Profile));
        
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
        _playerInput.actions["Join"].started += ctx =>
        {
            _gameStartManager.onPlayerHoldStart(gameObject);
        };

        _playerInput.actions["Join"].canceled += ctx =>
        {
            _gameStartManager.onPlayerLetGoStart(gameObject);
        };
    }
    
    private void SwitchToPage(Page page)
    {
        // Animation settings
        var ease = EaseType.BackOut;
        var duration = 0.2f;
        
        switch (page)
        {
            case Page.Main:
                UiUtil.DoRectTranslateAnim(mainPanel, new Vector2(_panelWidth, 0), duration, ease);
                UiUtil.DoRectTranslateAnim(profilePanel, new Vector2(_panelWidth, 0), duration, ease);
                eventSystem.SetSelectedGameObject(changeProfileButton.gameObject);
                break;
            case Page.Profile:
                UiUtil.DoRectTranslateAnim(mainPanel, new Vector2(-_panelWidth, 0), duration, ease);
                UiUtil.DoRectTranslateAnim(profilePanel, new Vector2(-_panelWidth, 0), duration, ease);
                eventSystem.SetSelectedGameObject(defaultProfileButton.gameObject);
                break;
        }
        
        _curPage = page;
    }
    
    enum Page { Main, Profile }
}
