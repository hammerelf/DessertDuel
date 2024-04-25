//Created by: Ryan King

using Sirenix.OdinInspector;
using UnityEngine;

namespace HammerElf.Games.DessertDuel
{
    public class DefensePlaceable : Placeable
    {
        [BoxGroup("Defense Only")]
        public int health;
        [BoxGroup("Defense Only")]
        public int damage;
        [BoxGroup("Defense Only")]
        public int attackRate;
    }
}
