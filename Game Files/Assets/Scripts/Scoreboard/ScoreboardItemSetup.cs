using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardItemSetup : MonoBehaviour
{
    [Header("Colors")]
    public Color firstPlaceColor;
    public Color secondPlaceColor;
    public Color thirdPlaceColor;
    public Color restPlaceColor;
    
    [Header("Setup parameters")]
    public PlayerProfileInfo playerProfileInfo;
    public int score;
    public int ranking;
    
    // UI elements
    public Image scoreAccent;
    public TMP_Text playerText;
    public TMP_Text pointsText;
    
    void Start()
    {
        playerText.text = playerProfileInfo.UseCustomName ? playerProfileInfo.CustomName : playerProfileInfo.Profile.name;
        var s = score != 1 ? "s" : "";
        pointsText.text = $"{score}pt{s}";

        switch (ranking)
        {
            case 1:
                scoreAccent.color = firstPlaceColor;
                break;
            case 2:
                scoreAccent.color = secondPlaceColor;
                break;
            case 3:
                scoreAccent.color = thirdPlaceColor;
                break;
            default:
                scoreAccent.color = restPlaceColor;
                break;
        }
    }
}
