//Created by: Ryan King

using Sirenix.OdinInspector;
using UnityEngine;

namespace HammerElf.Games.DessertDuel
{
    public class OffensePlaceable : Placeable
    {
        [BoxGroup("Offense Only")]
        public int moveSpeed;
        [BoxGroup("Offense Only")]
        public int health;
        [BoxGroup("Offense Only")]
        public int spawnRate;
    }
}
