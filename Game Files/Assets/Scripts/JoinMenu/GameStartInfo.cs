using System.Collections.Generic;
using UnityEngine;

namespace JoinMenu
{
    public class GameStartInfo : MonoBehaviour
    {
        public List<PlayerInfo> players = new();

        public class PlayerInfo
        {
            public Profile profile;

            public PlayerInfo(Profile profile)
            {
                this.profile = profile;
            }
        }
    }
}