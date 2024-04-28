//Created by: Ryan King

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
        [TitleGroup("Base")]
        public Image itemImage;
    }
}
