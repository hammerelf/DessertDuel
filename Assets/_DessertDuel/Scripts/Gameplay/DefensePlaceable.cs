//Created by: Ryan King

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
    }
}
