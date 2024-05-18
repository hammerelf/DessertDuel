//Created by: Ryan King

using HammerElf.Tools.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HammerElf.Games.DessertDuel
{
    public class OffensePlaceable : Placeable
    {
        [TitleGroup("Offense Only")]
        public int health;
        [TitleGroup("Offense Only")]
        public int moveSpeed;
        [TitleGroup("Offense Only")]
        public int spawnRate;

        public override string ToString()
        {
            return base.ToString() + 
                   "\nMove speed: " + moveSpeed +
                   "\nHealth: " + health +
                   "\nSpawn rate: " + spawnRate;
        }

        public OffensePlaceableJSON ToJSON()
        {
            return new OffensePlaceableJSON(id, itemName, cost, power, description, state, itemImagePath, health, moveSpeed, spawnRate);
        }

        public void SetFromJSON(OffensePlaceableJSON json)
        {
            if (json == null)
            {
                ConsoleLog.LogWarning("Deserialized json object is null.");
                return;
            }

            id = json.id;
            itemName = json.itemName;
            cost = json.cost;
            power = json.power;
            description = json.description;
            power = json.power;
            description = json.description;
            power = json.power;
            health = json.health;
            moveSpeed = json.moveSpeed;
            spawnRate = json.spawnRate;
        }

    }

    public class OffensePlaceableJSON : PlaceableJSON
    {
        public int health;
        public int moveSpeed;
        public int spawnRate;

        public OffensePlaceableJSON(string id, string itemName, int cost, int power, string description, PlaceableState state, string itemImagePath, int health, int moveSpeed, int spawnRate) : base(id, itemName, cost, power, description, state, itemImagePath)
        {
            this.isDefense = false;

            this.health = health;
            this.moveSpeed = moveSpeed;
            this.spawnRate = spawnRate;
        }
    }
}
