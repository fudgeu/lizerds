using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Scenes.Match
{
    public class PauseMenu : MonoBehaviour
    {
        [FormerlySerializedAs("ReturnButton")]
        public Button ContinueButton;
        public Button ExitToMenuButton;
    }
}
