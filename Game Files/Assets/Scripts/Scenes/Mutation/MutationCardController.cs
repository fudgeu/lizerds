using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MutationCardController : MonoBehaviour
{
    public string mutationLabel = "Mutation";
    public string mutationDescription = "Description";
    
    [Header("References")]
    public TMP_Text mutationLabelText;
    public TMP_Text mutationDescText;
    public Button button;
    
    void Start()
    {
        mutationLabelText.text = mutationLabel;
        mutationDescText.text = mutationDescription;
        
    }
}
