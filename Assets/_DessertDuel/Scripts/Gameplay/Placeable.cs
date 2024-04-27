//Created by: Ryan King

using UnityEngine;
using UnityEngine.UI;

namespace HammerElf.Games.DessertDuel
{
    public class Placeable : MonoBehaviour
    {
        public string id;
        public string itemName;
        public int cost;
        public int power;
        public string description;
        public PlaceableState state;
        public Image itemImage;
    }
}
