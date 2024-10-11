using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class ScrollChildController : MonoBehaviour
{
    private ScrollController _scrollController;
    private RectTransform _rectTransform;
    private MultiplayerEventSystem _multiplayerEventSystem;
    
    public int ItemIndex { get; set; }

    private bool _selectedFlag = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _scrollController = GetComponentInParent<ScrollController>();
        _multiplayerEventSystem = GetComponentInParent<EventSystemContainer>().multiplayerEventSystem;
    }

    // Update is called once per frame
    void Update()
    {
        var isSelected = _multiplayerEventSystem.currentSelectedGameObject == gameObject;
        if (!_selectedFlag && isSelected)
        {
            _selectedFlag = true;
            OnSelected();
        }
        else if (_selectedFlag && !isSelected)
        {
            _selectedFlag = false;
        }
    }

    private void OnSelected()
    {
        _scrollController.ScrollTo(_rectTransform, ItemIndex);
    }
}
