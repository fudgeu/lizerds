using Tweens;
using UnityEngine;

public class UiUtil
{
    public static void DoRectTranslateAnim(RectTransform rectTransform, Vector2 delta,
        float duration = 0.2f, EaseType easeType = EaseType.Linear)
    {
        // Setup tweens
        var tween1 = new Vector2Tween
        {
            from = rectTransform.offsetMin,
            to = rectTransform.offsetMin + delta,
            duration = duration,
            easeType = easeType,
            onUpdate = (instance, value) => rectTransform.offsetMin = value,
        };
        var tween2 = new Vector2Tween
        {
            from = rectTransform.offsetMax,
            to = rectTransform.offsetMax + delta,
            duration = duration,
            easeType = easeType,
            onUpdate = (instance, value) => rectTransform.offsetMax = value,
        };
        
        // Apply tweens
        rectTransform.gameObject.AddTween(tween1);
        rectTransform.gameObject.AddTween(tween2);
    }

    public static void SetPos(RectTransform rectTransform, Vector2 pos)
    {
        var width = rectTransform.offsetMax.x - rectTransform.offsetMin.x;
        var height = rectTransform.offsetMax.y - rectTransform.offsetMin.y;
    }
}
