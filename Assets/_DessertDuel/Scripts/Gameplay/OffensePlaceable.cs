//Created by: Ryan King

using Sirenix.OdinInspector;
using UnityEngine;

namespace HammerElf.Games.DessertDuel
{
    public class OffensePlaceable : Placeable
    {
        [TitleGroup("Offense Only")]
        public int moveSpeed;
        [TitleGroup("Offense Only")]
        public int health;
        [TitleGroup("Offense Only")]
        public int spawnRate;
    }
}
