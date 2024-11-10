using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tweens;
using UnityEngine;
using UnityEngine.Serialization;

public class EndRoundScoreboardSetup : MonoBehaviour
{
    [Header("Setup parameters")]
    public Dictionary<PlayerProfileInfo, int> roundScoreboard;
    public Dictionary<PlayerProfileInfo, int> gameScoreboard;

    [Header("Internal UI elements")]
    public GameObject scoresPanel;
    
    [FormerlySerializedAs("scoreUIObject")]
    [Header("Prefabs")]
    public GameObject scoreItemPrefab;
    
    // Events
    public delegate void ScoreboardFinishedDelegate();
    public event ScoreboardFinishedDelegate ScoreboardFinished;
    
    void Start()
    {
        var sortedRoundScoreboard = roundScoreboard.OrderBy(entry => -entry.Value);
        
        // Create score item for each player
        int i = 1;
        foreach (var entry in sortedRoundScoreboard)
        {
            var scoreItem = Instantiate(scoreItemPrefab, scoresPanel.transform);
            var scoreItemSetup = scoreItem.GetComponent<ScoreboardItemSetup>();
            scoreItemSetup.playerProfileInfo = entry.Key;
            scoreItemSetup.score = entry.Value;
            scoreItemSetup.ranking = i;
            i++;
        }

        StartCoroutine(StartTimeout());
    }

    private IEnumerator StartTimeout()
    {
        yield return new WaitForSeconds(5);
        ScoreboardFinished?.Invoke();
    }
}
