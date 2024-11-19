using System.Collections.Generic;
using UnityEngine;

namespace JoinMenu
{
    public class GameStartInfo : MonoBehaviour
    {
        public List<GameObject> players = new();
        public List<string> arenas = new();
        public List<string> gameModes = new();

        public void AttachRandomGameMode(GameObject gameModeObject)
        {
            var gameMode = gameModes[Random.Range(0, gameModes.Count)];

            switch (gameMode)
            {
                case "Natural Selection":
                    gameModeObject.AddComponent<NaturalSelectionGameMode>();
                    return;
                case "Predator":
                    gameModeObject.AddComponent<PredatorGameMode>();
                    return;
                default:
                    return;
            }
        }
    }
}