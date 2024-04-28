//Created by: Ryan King

using HammerElf.Tools.Utilities;
using UnityEngine;

namespace HammerElf.Games.DessertDuel
{
    public class GameManager : SerializedSingleton<GameManager>
    {
        public PlayerState currentPlayerState = new PlayerState();
        public StoreController storeController;
    }
}
