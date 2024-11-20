using Scenes.Match.GameModes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tweens;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchIntroScreen : MonoBehaviour
{
    public TMP_Text arenaName;
    public TMP_Text gameModeName;
    
    // Start is called before the first frame update
    void Start()
    {
        arenaName.text = SceneManager.GetActiveScene().name;
        gameModeName.text = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>().gameModeName;
        
        var canvasGroup = GetComponentInChildren<CanvasGroup>();
        
        // Modify opacity and destroy self after 4 seconds
        var tween = new FloatTween
        {
            from = 1f,
            to = 0f,
            duration = 2,
            delay = 2,
            easeType = EaseType.QuadInOut,
            onUpdate = (_, newVal) => canvasGroup.alpha = newVal,
            onEnd = (_) => Destroy(gameObject),
        };
        gameObject.AddTween(tween);
    }
}
