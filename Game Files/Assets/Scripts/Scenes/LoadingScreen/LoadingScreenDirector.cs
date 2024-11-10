using UnityEngine;

namespace Scenes
{
    public class LoadingScreenDirector : MonoBehaviour
    {
        public GameScene goTo = GameScene.MainMenu;
        
        public enum GameScene
        {
            MainMenu,
            MatchSetup,
            Arena,
            Results,
        }
    }
}