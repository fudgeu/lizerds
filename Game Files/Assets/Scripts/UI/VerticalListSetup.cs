using UnityEngine;
using UnityEngine.UI;

// Set up a vertical list by linking all buttons together and indexing them
public class VerticalListSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Get all buttons
        var buttons = GetComponentsInChildren<Button>();
        
        Button lastButton = null;
        for (int i = 0; i < buttons.Length; i++)
        {
            var button = buttons[i];

            // Set index
            var scrollChildController = button.GetComponent<ScrollChildController>();
            if (scrollChildController != null)
            {
                scrollChildController.ItemIndex = i;
            }
            
            // Link navigation
            var nav = button.navigation;

            if (i + 1 < buttons.Length)
            {
                nav.selectOnDown = buttons[i + 1];
            }

            if (lastButton != null)
            {
                nav.selectOnUp = lastButton;
            }
            
            button.navigation = nav;
            lastButton = button;
        }
    }
}
