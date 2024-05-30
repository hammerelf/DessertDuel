//Created by: Ryan King

//TODO: Create battle scene. Visualize attacks per second on battle scene with UI text. Then continue refining attacks per second function.

using HammerElf.Tools.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HammerElf.Games.DessertDuel
{
    //For now just worry about player defense and opponent offense. Do math for one side of battle
    //then mirror logic to figure out which side wins the battle.
    public class BattleController : Singleton<BattleController>
    {
        [SerializeField]
        private List<DefensePlaceable> defenseOptions;
        [SerializeField]
        private List<OffensePlaceable> offenseOptions;

        private Lane[] playerLanes = new Lane[3], opponentLanes = new Lane[3];
        private List<EnemyUnit> playerEnemies = new(), opponentEnemies = new();
        private PlayerState playerState;

        [SerializeField]
        private int battleDelay = 5;
        public int laneDistance = 10;
        private float timeTracking;
        private bool isPlayerLost;

        [SerializeField]
        private GameObject[] playerLaneEnemyHolders = new GameObject[3], 
                             opponentLaneEnemyHolders = new GameObject[3];
        [SerializeField]
        private GameObject enemyVisualizer;

        protected override void Awake()
        {
            base.Awake();
            GameManager.Instance.battleController = this;
        }

        private void Start()
        {
            playerState.lanes = playerLanes;

            PopulateOpponentLanesWithRandom();

            for(int i = 0; i < playerLanes.Length; i++)
            {
                if (playerLanes[i].offenseSlot1 != null) opponentEnemies.Add(new(playerLanes[i].offenseSlot1, i));
                if (playerLanes[i].offenseSlot2 != null) opponentEnemies.Add(new(playerLanes[i].offenseSlot2, i));
                if (playerLanes[i].offenseSlot3 != null) opponentEnemies.Add(new(playerLanes[i].offenseSlot3, i));
            }

            for (int i = 0; i < opponentLanes.Length; i++)
            {
                if (opponentLanes[i].offenseSlot1 != null)
                {
                    playerEnemies.Add(new(opponentLanes[i].offenseSlot1, i));
                    GameObject.Instantiate(enemyVisualizer);
                }
                if (opponentLanes[i].offenseSlot2 != null) playerEnemies.Add(new(opponentLanes[i].offenseSlot2, i));
                if (opponentLanes[i].offenseSlot3 != null) playerEnemies.Add(new(opponentLanes[i].offenseSlot3, i));
            }

            foreach(EnemyUnit enemy in playerEnemies)
            {

            }

            //This will actually need to be called when battle starts instead of here. This is what
            //makes calculations occur once per second for attackers.
            InvokeRepeating("CalculateAttackersPerSecond", battleDelay, 1);
        }

        //Temporary function to fill in for opponents. Will fill opponent lane with one defender each
        //and a random number of attackers weighted to lower amounts except for first lane which will
        //always have at least one.
        private void PopulateOpponentLanesWithRandom()
        {
            List<OffensePlaceable> offenses = GameManager.Instance.offenseOptions;
            foreach(Lane lane in opponentLanes)
            {
                int offenseLaneOdds = Random.Range(0 + GameManager.Instance.currentPlayerState.wins + 
                                                   GameManager.Instance.currentPlayerState.losses, 100);

                lane.defenseSlot = GameManager.Instance.defenseOptions[Random.Range(0, GameManager.Instance.defenseOptions.Count)];
                if(offenseLaneOdds >= 30 || opponentLanes[0] == lane) lane.offenseSlot1 = offenses[Random.Range(0,offenses.Count)];
                if (offenseLaneOdds >= 60) lane.offenseSlot2 = offenses[Random.Range(0, offenses.Count)];
                if (offenseLaneOdds >= 80) lane.offenseSlot3 = offenses[Random.Range(0, offenses.Count)];
            }
        }

        //Every second, calculate the movement of each attacking unit and if they make it to the end 
        //of a lane then subtract one health from the defending unit and reset their position to the
        //start of the lane for them to begin moving forward again. This stops when the attacking unit
        //reaches the defending unit and reduces it to 0 health or when the attacking unit has no
        //health remaining.
        private void CalculateAttackersPerSecond()
        {
            foreach(EnemyUnit enemy in playerEnemies)
            {
                if(enemy.health <= 0)
                {
                    playerEnemies.Remove(enemy);
                    continue;
                }

                if (enemy.distance <= 0)
                {
                    if (playerLanes[enemy.laneNumber].defenseSlot.health > 1)
                    {
                        enemy.distance = laneDistance;
                    }
                    else
                    {
                        playerEnemies.Remove(enemy);
                    }
                    playerLanes[enemy.laneNumber].defenseSlot.health -= 1;
                    continue;
                }

                enemy.distance -= enemy.enemyType.moveSpeed;
                if (enemy.distance < 0)
                {
                    enemy.distance = 0;
                }
            }

            CheckForLoss();
        }

        //If all defense units for the player have 0 health then the player loses.
        //Although I think I had a different idea for the losing conditions.
        private void CheckForLoss()
        {
            bool lossCheck = false;
            foreach(Lane lane in playerLanes)
            {
                if(lane.defenseSlot.health > 0)
                {
                    lossCheck = true;
                }
            }

            isPlayerLost = lossCheck;
        }
    }

    class EnemyUnit
    {
        public OffensePlaceable enemyType;
        public int distance;
        public int laneNumber;
        public int health;

        public EnemyUnit()
        {
            enemyType = null;
            distance = BattleController.Instance.laneDistance;
            health = enemyType != null ? enemyType.health : 0;
        }

        public EnemyUnit(OffensePlaceable offensePlaceable, int lane)
        {
            enemyType = offensePlaceable;
            distance = BattleController.Instance.laneDistance;
            laneNumber = lane;
            health = offensePlaceable.health;
        }
    }
}
