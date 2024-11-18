using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MutationCardController : MonoBehaviour
{
    public string mutationLabel = "Mutation";
    
    [Header("References")]
    public TMP_Text mutationLabelText;
    public Button button;
    
    void Start()
    {
        mutationLabelText.text = mutationLabel;
        
    }
}
