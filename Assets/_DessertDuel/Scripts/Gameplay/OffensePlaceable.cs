//Created by: Ryan King

using HammerElf.Tools.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

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

        public void SetAllFields(OffensePlaceable other)
        {
            this.health = other.health;
            this.moveSpeed = other.moveSpeed;
            this.spawnRate = other.spawnRate;
            this.itemName = other.itemName;
            this.cost = other.cost;
            this.power = other.power;
            this.description = other.description;
            this.state = other.state;
            if (other.itemImage != null) this.itemImage = other.itemImage;
            this.itemImagePath = other.itemImagePath;
        }

        public void SetAllFields(int health, int moveSpeed, int spawnRate, string id, string itemName, int cost,
                                 int power, string description, PlaceableState state, string itemImagePath, Image itemImage = null)
        {
            this.health = health;
            this.moveSpeed = moveSpeed;
            this.spawnRate = spawnRate;
            this.itemName = itemName;
            this.cost = cost;
            this.power = power;
            this.description = description;
            this.state = state;
            if(itemImage != null) this.itemImage = itemImage;
            this.itemImagePath = itemImagePath;
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
