//Created by: Ryan King

using HammerElf.Tools.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

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

        public void SetFromJSON(DefensePlaceableJSON json)
        {
            if(json == null)
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
