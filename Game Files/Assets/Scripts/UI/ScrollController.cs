using System;
using System.Collections;
using System.Collections.Generic;
using Tweens;
using Tweens.Core;
using UnityEngine;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour
{
    private ScrollRect _scrollRect;
    public RectTransform contentPanel;
    public RectTransform viewPanel;
    
    private VerticalLayoutGroup _verticalLayoutGroup;
    
    public void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _verticalLayoutGroup = contentPanel.GetComponent<VerticalLayoutGroup>();
    }
    
    public void ScrollTo(RectTransform target, int index = -1)
    { 
        Canvas.ForceUpdateCanvases();
        
        var containerTop = contentPanel.transform.localPosition.y;
        var containerBottom = contentPanel.transform.localPosition.y + viewPanel.rect.height;
        var targetTopBorder = Math.Abs(target.localPosition.y);
        var targetBottomBorder = targetTopBorder + target.rect.height;
        
        if (index == 0)
        {
            // Item is first in list, scroll all the way to the top
            var contentPos = contentPanel.transform.localPosition;
            var tween = new Vector2Tween
            {
                from = new Vector2(contentPos.x, contentPos.y),
                to = new Vector2(contentPos.x, 0),
                duration = 0.15f,
                onUpdate = (instance, value) => contentPanel.transform.localPosition = value,
                easeType = EaseType.QuadOut,
            };
            contentPanel.gameObject.AddTween(tween);
        }
        else if (targetBottomBorder > containerBottom)
        {
            // Item border is lower than container
            var contentPos = contentPanel.transform.localPosition;
            var tween = new Vector2Tween
            {
                from = new Vector2(contentPos.x, contentPos.y),
                to = new Vector2(contentPos.x, targetBottomBorder - viewPanel.rect.height + _verticalLayoutGroup.padding.bottom),
                duration = 0.15f,
                onUpdate = (instance, value) => contentPanel.transform.localPosition = value,
                easeType = EaseType.QuadOut,
            };
            contentPanel.gameObject.AddTween(tween);
        }
        else if (targetTopBorder < containerTop)
        {
            // Item border is higher than container
            var contentPos = contentPanel.transform.localPosition;
            var tween = new Vector2Tween
            {
                from = new Vector2(contentPos.x, contentPos.y),
                to = new Vector2(contentPos.x, targetTopBorder - _verticalLayoutGroup.padding.top),
                duration = 0.15f,
                onUpdate = (instance, value) => contentPanel.transform.localPosition = value,
                easeType = EaseType.QuadOut,
            };
            contentPanel.gameObject.AddTween(tween);
        }
    }
}
