//Created by: Ryan King

//TODO: Change this into ScriptableObject.

using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace HammerElf.Games.DessertDuel
{
    public class Placeable : MonoBehaviour
    {
        [TitleGroup("Base")]
        public string id;
        [TitleGroup("Base")]
        public string itemName;
        [TitleGroup("Base")]
        public int cost;
        [TitleGroup("Base")]
        public int power;
        [TitleGroup("Base")]
        public string description;
        [TitleGroup("Base")]
        public PlaceableState state;
        [TitleGroup("Base"), JsonIgnore]
        public Image itemImage;
        [TitleGroup("Base")]
        public string itemImagePath;

        public override string ToString()
        {
            return "Placeable id: " + id +
                   "\nItem name: " + itemName +
                   "\nCost: " + cost +
                   "\nPower: " + power +
                   "\nDescription: " + description +
                   "\nCurrent state: " + state.ToString() + 
                   "\nItem image path: " + itemImagePath;
        }

        //public DefensePlaceableJSON ToJSON()
        //{
        //    return new DefensePlaceableJSON(id, itemName, cost, power, description, state, itemImagePath, health, damage, attackRate);
        //}
    }

    public class PlaceableJSON
    {
        public bool isDefense;
        public string id;
        public string itemName;
        public int cost;
        public int power;
        public string description;
        public PlaceableState state;
        public string itemImagePath;

        public PlaceableJSON(string id, string itemName, int cost, int power, string description, PlaceableState state, string itemImagePath)
        {
            this.id = id;
            this.itemName = itemName;
            this.cost = cost;
            this.power = power;
            this.description = description;
            this.state = state;
            this.itemImagePath = itemImagePath;
        }
    }
}
