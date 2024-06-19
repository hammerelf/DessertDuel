//Created by: Ryan King

using HammerElf.Tools.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HammerElf.Games.DessertDuel
{
    public class PlayerState
    {
        public int money;
        public int wins;
        public int losses;
        public int lives;
        public int currentRound;
        public Lane[] lanes = new Lane[3];
        public List<OffensePlaceable> offensePlaceables;
        public List<DefensePlaceable> defensePlaceables;
        public List<OffensePlaceable> offenseStore;
        public List<DefensePlaceable> defenseStore;

        public List<string> offensePlacement, defensePlacement;

        public PlayerState()
        {
            money = 0;
            wins = 0;
            losses = 0;
            lives = 5;
            currentRound = 0;
            lanes = new Lane[3];
            offensePlaceables = new List<OffensePlaceable>();
            defensePlaceables = new List<DefensePlaceable>();
            offenseStore = new List<OffensePlaceable>();
            defenseStore = new List<DefensePlaceable>();
            offensePlacement = new List<string>();
            defensePlacement = new List<string>();
        }

        public PlayerState(int money, int wins, int losses, int lives, int currentRound, List<Lane> placementLanes, 
                           List<OffensePlaceable> offensePlaceables, List<DefensePlaceable> defensePlaceables,
                           List<OffensePlaceable> offenseStore, List<DefensePlaceable> defenseStore,
                           List<string> offensePlacement, List<string> defensePlacement)
        {
            this.money = money;
            this.wins = wins;
            this.losses = losses;
            this.lives = lives;
            this.currentRound = currentRound;
            this.lanes = lanes ?? throw new ArgumentNullException(nameof(placementLanes));
            this.offensePlaceables = offensePlaceables ?? throw new ArgumentNullException(nameof(offensePlaceables));
            this.defensePlaceables = defensePlaceables ?? throw new ArgumentNullException(nameof(defensePlaceables));
            this.offenseStore = offenseStore ?? throw new ArgumentNullException(nameof(offenseStore));
            this.defenseStore = defenseStore ?? throw new ArgumentNullException(nameof(defenseStore));
            this.offensePlacement = offensePlacement ?? throw new ArgumentException(nameof(offensePlacement));
            this.defensePlacement = defensePlacement ?? throw new ArgumentException(nameof(defensePlacement));
        }

        public void LoadPlayerState(string jsonState)
        {
            JsonConvert.DeserializeObject<PlayerState>(jsonState);
        }
    }

    //This will most likely be replacing PlayerState since it is more easily saved to json.
    public class JsonPlayerState
    {
        public int money;
        public int wins;
        public int losses;
        public int lives;
        public int currentRound;
        public string[] offensePlacements1, offensePlacements2, offensePlacements3;
        public string defensePlacement1, defensePlacement2, defensePlacement3;
        public List<string> offenseStore = new(), defenseStore = new();
        public List<string> offenseStorage = new(), defenseStorage = new();

        public JsonPlayerState()
        {
            this.money = 0;
            this.wins = 0;
            this.losses = 0;
            this.lives = 0;
            this.currentRound = 0;
            this.offensePlacements1 = new string[] { "", "", "" };
            this.offensePlacements2 = new string[] { "", "", "" };
            this.offensePlacements3 = new string[] { "", "", "" };
            this.defensePlacement1 = "";
            this.defensePlacement2 = "";
            this.defensePlacement3 = "";
            this.offenseStore = new List<string>();
            this.defenseStore = new List<string>();
            this.offenseStorage = new List<string>();
            this.defenseStorage = new List<string>();
        }

        public JsonPlayerState(int money, int wins, int losses, int lives, int currentRound,
                               string[] offensePlacement1, string[] offensePlacement2,
                               string[] offensePlacement3, string defensePlacement1,
                               string defensePlacement2, string defensePlacement3,
                               List<string> offenseStore, List<string> defenseStore, List<string> offenseStorage, List<string> defenseStorage)
        {
            this.money = money;
            this.wins = wins;
            this.losses = losses;
            this.lives = lives;
            this.currentRound = currentRound;
            this.offensePlacements1 = offensePlacement1;
            this.offensePlacements2 = offensePlacement2;
            this.offensePlacements3 = offensePlacement3;
            this.defensePlacement1 = defensePlacement1;
            this.defensePlacement2 = defensePlacement2;
            this.defensePlacement3 = defensePlacement3;
            this.offenseStore = offenseStore;
            this.defenseStore = defenseStore;
            this.offenseStorage = offenseStorage;
            this.defenseStorage = defenseStorage;
        }

        public JsonPlayerState LoadPlayerState(string jsonState)
        {
            return JsonConvert.DeserializeObject<JsonPlayerState>(jsonState);
        }

        public string PlayerStateToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void SaveJson(string jsonData)
        {
            string path = GameManager.Instance.jsonOutputFolderPath + "PlayerState.json";
            File.WriteAllText(path, jsonData);
            ConsoleLog.Log("Player data saved. Exists: " + File.Exists(path));
        }
    }
}
