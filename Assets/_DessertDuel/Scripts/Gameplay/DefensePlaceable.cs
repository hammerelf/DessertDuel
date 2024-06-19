//Created by: Ryan King

using HammerElf.Tools.Utilities;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace HammerElf.Games.DessertDuel
{
    public class DefensePlaceable : Placeable
    {
        [TitleGroup("Defense Only")]
        public int health;
        [TitleGroup("Defense Only")]
        public int damage;
        [TitleGroup("Defense Only")]
        public int attackRate;

        public override string ToString()
        {
            return base.ToString() +
                   "\nHealth: " + health +
                   "\nDamage: " + damage +
                   "\nAttack Rate: " + attackRate;
        }

        public DefensePlaceableJSON ToJSON()
        {
            return new DefensePlaceableJSON(id, itemName, cost, power, description, state, itemImagePath, health, damage, attackRate);
        }

        public void SetAllFields(DefensePlaceable other)
        {
            this.health = other.health;
            this.damage = other.damage;
            this.attackRate = other.attackRate;
            this.itemName = other.itemName;
            this.cost = other.cost;
            this.power = other.power;
            this.description = other.description;
            this.state = other.state;
            if (other.itemImage != null) this.itemImage = other.itemImage;
            this.itemImagePath = other.itemImagePath;
        }

        public void SetAllFields(int health, int damage, int attackRate, string id, string itemName, int cost,
                                 int power, string description, PlaceableState state, string itemImagePath, Image itemImage = null)
        {
            this.health = health;
            this.damage = damage;
            this.attackRate = attackRate;
            this.itemName = itemName;
            this.cost = cost;
            this.power = power;
            this.description = description;
            this.state = state;
            if (itemImage != null) this.itemImage = itemImage;
            this.itemImagePath = itemImagePath;
        }

        public void SetFromJSON(DefensePlaceableJSON json)
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
            state = json.state;
            itemImagePath = json.itemImagePath;
            health = json.health;
            damage = json.damage;
            attackRate = json.attackRate;
        }

        public void LoadFromId(string id)
        {
            string path = GameManager.Instance.jsonOutputFolderPath + "DP_" + id + ".json";
            string data = "";
            if (File.Exists(path))
            {
                data = File.ReadAllText(path);
            }
            if (data.Equals(""))
            {
                ConsoleLog.LogWarning("File not found for id: " + id);
                return;
            }

            SetFromJSON(JsonConvert.DeserializeObject<DefensePlaceableJSON>(data));
        }
    }

    public class DefensePlaceableJSON : PlaceableJSON
    {
        public int health;
        public int damage;
        public int attackRate;

        public DefensePlaceableJSON(string id, string itemName, int cost, int power, string description, PlaceableState state, string itemImagePath, int health, int damage, int attackRate) : base(id, itemName, cost, power, description, state, itemImagePath)
        {
            this.isDefense = true;

            this.health = health;
            this.damage = damage;
            this.attackRate = attackRate;
        }
    }
}
