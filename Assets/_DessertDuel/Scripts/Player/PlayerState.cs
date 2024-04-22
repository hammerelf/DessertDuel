//Created by: Ryan King

using System;
using System.Collections.Generic;
using UnityEngine;

namespace HammerElf.Games.DessertDuel
{
    public class PlayerState
    {
        public int money;
        public int wins;
        public int lives;
        public int currentRound;
        public List<Lane> placementLanes;
        public List<OffensePlaceable> offensePlaceables;
        public List<DefensePlaceable> defensePlaceables;
        public List<OffensePlaceable> offenseStore;
        public List<DefensePlaceable> defenseStore;

        public PlayerState()
        {
            money = 0;
            wins = 0;
            lives = 5;
            currentRound = 0;
            placementLanes = new List<Lane>();
            offensePlaceables = new List<OffensePlaceable>();
            defensePlaceables = new List<DefensePlaceable>();
            offenseStore = new List<OffensePlaceable>();
            defenseStore = new List<DefensePlaceable>();
        }

        public PlayerState(int money, int wins, int lives, int currentRound, List<Lane> placementLanes, 
                           List<OffensePlaceable> offensePlaceables, List<DefensePlaceable> defensePlaceables,
                           List<OffensePlaceable> offenseStore, List<DefensePlaceable> defenseStore)
        {
            this.money = money;
            this.wins = wins;
            this.lives = lives;
            this.currentRound = currentRound;
            this.placementLanes = placementLanes ?? throw new ArgumentNullException(nameof(placementLanes));
            this.offensePlaceables = offensePlaceables ?? throw new ArgumentNullException(nameof(offensePlaceables));
            this.defensePlaceables = defensePlaceables ?? throw new ArgumentNullException(nameof(defensePlaceables));
            this.offenseStore = offenseStore ?? throw new ArgumentNullException(nameof(offenseStore));
            this.defenseStore = defenseStore ?? throw new ArgumentNullException(nameof(defenseStore));
        }
    }
}
