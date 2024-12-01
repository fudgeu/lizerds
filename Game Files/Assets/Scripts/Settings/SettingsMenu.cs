using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Button doneButton;
    
    void Start()
    {
        var prevSelected = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(volumeSlider.gameObject);
        doneButton.onClick.AddListener(() =>
        {
            EventSystem.current.SetSelectedGameObject(prevSelected);
            Destroy(transform.parent.gameObject);
        });
    }
}
