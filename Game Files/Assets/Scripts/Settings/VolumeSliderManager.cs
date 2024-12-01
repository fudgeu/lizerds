using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderManager : MonoBehaviour
{
    public Slider volumeSlider;
    
    void Start()
    {
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(HandleOnVolumeChanged);        
    }

    private void HandleOnVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }
}
