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

        public override string ToString()
        {
            return base.ToString() + 
                   "\nMove speed: " + moveSpeed +
                   "\nHealth: " + health +
                   "\nSpawn rate: " + spawnRate;
        }
    }
}
