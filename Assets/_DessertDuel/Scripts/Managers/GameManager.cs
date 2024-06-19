//Created by: Ryan King

using HammerElf.Tools.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HammerElf.Games.DessertDuel
{
    public class GameManager : SerializedSingleton<GameManager>
    {
        public PlayerState currentPlayerState = new PlayerState();
        public JsonPlayerState jPlayerState = new JsonPlayerState();
        public SplashController splashController;
        public StoreController storeController;
        public BattleController battleController;

        public List<DefensePlaceable> defenseOptions;
        public List<OffensePlaceable> offenseOptions;
        [HideInInspector]
        public string jsonOutputFolderPath = "Assets/_DessertDuel/Resources/JSON/";

        public void LoadScene(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
