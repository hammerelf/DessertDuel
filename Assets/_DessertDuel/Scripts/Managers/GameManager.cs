//Created by: Ryan King

using HammerElf.Tools.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace HammerElf.Games.DessertDuel
{
    public class GameManager : SerializedSingleton<GameManager>
    {
        public PlayerState currentPlayerState = new PlayerState();
        public SplashController splashController;
        public StoreController storeController;
        public BattleController battleController;

        public List<DefensePlaceable> defenseOptions;
        public List<OffensePlaceable> offenseOptions;
    }
}
