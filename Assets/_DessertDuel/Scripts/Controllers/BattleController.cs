//Created by: Ryan King

//TODO: Visualizer values are no longer updating. Continue refining attacks per second function.

using HammerElf.Tools.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HammerElf.Games.DessertDuel
{
    //For now just worry about player defense and opponent offense. Do math for one side of battle
    //then mirror logic to figure out which side wins the battle.
    public class BattleController : Singleton<BattleController>
    {
        private Lane[] playerLanes = new Lane[3], opponentLanes = new Lane[3];
        private List<AttackVisualizer> playerEnemies = new(), opponentEnemies = new();
        private List<DefenseVisualizer> playerDefenders = new(), opponentDefenders = new();
        private PlayerState playerState;

        [SerializeField]
        private int battleDelay = 5;
        public int laneDistance = 10;
        private float timeTracking;
        private bool isPlayerLost;

        [SerializeField]
        private GameObject[] playerLaneEnemyHolders = new GameObject[3], 
                             opponentLaneEnemyHolders = new GameObject[3],
                             playerLaneDefenseHolders = new GameObject[3],
                             opponentLaneDefenseHolders = new GameObject[3];
        [SerializeField]
        private GameObject enemyVisualizer, defenseVisualizer;

        protected override void Awake()
        {
            base.Awake();
            GameManager.Instance.battleController = this;
        }

        private void Start()
        {
            //playerState.lanes = playerLanes;

            PopulateOpponentLanesWithRandom();

            //TODO: Remove because temporary to test lane population
            playerLanes = opponentLanes;

            //for(int i = 0; i < playerLanes.Length; i++)
            //{
            //    if (playerLanes[i].offenseSlot1 != null) opponentEnemies.Add(new(playerLanes[i].offenseSlot1, i));
            //    if (playerLanes[i].offenseSlot2 != null) opponentEnemies.Add(new(playerLanes[i].offenseSlot2, i));
            //    if (playerLanes[i].offenseSlot3 != null) opponentEnemies.Add(new(playerLanes[i].offenseSlot3, i));
            //}

            for (int i = 0; i < opponentLanes.Length; i++)
            {
                OffensePlaceable[] offenseSlotGetter = new OffensePlaceable[]
                { opponentLanes[i].offenseSlot1, opponentLanes[i].offenseSlot2, opponentLanes[i].offenseSlot3 };

                for(int j = 0; j < offenseSlotGetter.Length; j++)
                {
                    if (offenseSlotGetter[j] != null)
                    {
                        GameObject go = GameObject.Instantiate(enemyVisualizer, playerLaneEnemyHolders[i].transform);
                        AttackVisualizer visual = go.GetComponent<AttackVisualizer>();
                        visual.enemyType.SetAllFields(offenseSlotGetter[j]);
                        visual.laneNumber = i;
                        visual.health = offenseSlotGetter[j].health;
                        playerEnemies.Add(visual);
                    }
                }
            }

            for(int i = 0; i < playerLaneDefenseHolders.Length; i++)
            {
                GameObject defGO = Instantiate(defenseVisualizer, playerLaneDefenseHolders[i].transform);
                DefenseVisualizer defVisual = defGO.GetComponent<DefenseVisualizer>();
                defVisual.defenseType.SetAllFields(GameManager.Instance.defenseOptions[Random.Range(0, GameManager.Instance.defenseOptions.Count)]);
                //ConsoleLog.Log("Defender type image name: " + defVisual.defenseType.itemImage.sprite.name);
                playerLanes[i].defenseSlot = defVisual.defenseType;
                defVisual.laneNumber = i;
                defVisual.Start();
                //ConsoleLog.Log("Defender visualizer image name: " + defVisual.image.sprite.name);
                playerDefenders.Add(defVisual);
                ConsoleLog.Log("Defense slot health: " + defVisual.defenseType.health);
            }

            //This will actually need to be called when battle starts instead of here. This is what
            //makes calculations occur once per second for attackers.
            InvokeRepeating("CalculateAttackersPerSecond", battleDelay, 1);

            //Have each defender make damage calls on repeat.
            foreach(DefenseVisualizer defender in playerDefenders)
            {
                StartCoroutine(DefenderDamage(defender.defenseType.attackRate, defender.laneNumber, defender.defenseType.damage));
            }
        }

        //Temporary function to fill in for opponents. Will fill opponent lane with one defender each
        //and a random number of attackers weighted to lower amounts except for first lane which will
        //always have at least one.
        private void PopulateOpponentLanesWithRandom()
        {
            opponentLanes[0] = new Lane();
            opponentLanes[1] = new Lane();
            opponentLanes[2] = new Lane();

            List<OffensePlaceable> offenses = GameManager.Instance.offenseOptions;
            for (int i = 0; i < opponentLanes.Length; i++)
            {
                GameObject defGO = Instantiate(defenseVisualizer, opponentLaneDefenseHolders[i].transform);
                DefenseVisualizer defVisual = defGO.GetComponent<DefenseVisualizer>();
                defVisual.defenseType.SetAllFields(GameManager.Instance.defenseOptions[Random.Range(0, GameManager.Instance.defenseOptions.Count)]);
                opponentLanes[i].defenseSlot = defVisual.defenseType;

                int offenseLaneOdds = Random.Range(0 + GameManager.Instance.currentPlayerState.wins + 
                                                   GameManager.Instance.currentPlayerState.losses, 100);
                if(offenseLaneOdds >= 0 || opponentLanes[0] == opponentLanes[i]) opponentLanes[i].offenseSlot1 = offenses[Random.Range(0,offenses.Count)];
                if(offenseLaneOdds >= 60) opponentLanes[i].offenseSlot2 = offenses[Random.Range(0, offenses.Count)];
                if(offenseLaneOdds >= 80) opponentLanes[i].offenseSlot3 = offenses[Random.Range(0, offenses.Count)];
            }
        }

        //Every second, calculate the movement of each attacking unit and if they make it to the end 
        //of a lane then subtract one health from the defending unit and reset their position to the
        //start of the lane for them to begin moving forward again. This stops when the attacking unit
        //reaches the defending unit and reduces it to 0 health or when the attacking unit has no
        //health remaining.
        private void CalculateAttackersPerSecond()
        {
            for(int i = 0; i < playerEnemies.Count; i++)
            //foreach(AttackVisualizer enemy in playerEnemies)
            {
                playerEnemies[i].distance -= playerEnemies[i].enemyType.moveSpeed;

                if (playerEnemies[i].health <= 0)
                {
                    playerEnemies.Remove(playerEnemies[i]);
                    continue;
                }

                if (playerEnemies[i].distance <= 0)
                {
                    if (playerEnemies[i].health > 1)
                    {
                        playerEnemies[i].distance = laneDistance;
                        playerDefenders[playerEnemies[i].laneNumber].health -= 1;
                    }
                    //if (playerLanes[playerEnemies[i].laneNumber].defenseSlot.health > 1)
                    //{
                    //    playerEnemies[i].distance = laneDistance;
                    //    playerLanes[playerEnemies[i].laneNumber].defenseSlot.health -= 1;
                    //}
                    else
                    {
                        playerEnemies[i].gameObject.SetActive(false);
                        //playerEnemies.Remove(playerEnemies[i]);
                    }
                    //continue;
                }

                playerEnemies[i].distance -= playerEnemies[i].enemyType.moveSpeed;
                if (playerEnemies[i].distance < 0)
                {
                    playerEnemies[i].distance = 0;
                }
            }

            CheckForLoss();
        }

        //If all defense units for the player have 0 health then the player loses.
        //Although I think I had a different idea for the losing conditions.
        private void CheckForLoss()
        {
            bool lossCheck = true;
            foreach(Lane lane in playerLanes)
            {
                if(lane.defenseSlot.health > 0)
                {
                    lossCheck = false;
                }
            }
            if (lossCheck) ConsoleLog.Log("Player lost all defense units!");
            isPlayerLost = lossCheck;
        }

        private IEnumerator DefenderDamage(int rate, int lane, int damage)
        {
            yield return new WaitForSeconds(battleDelay);

            do
            {
                foreach (AttackVisualizer visual in playerEnemies)
                {
                    if (visual.laneNumber.Equals(lane))
                    {
                        visual.health -= damage;
                    }
                }

                yield return new WaitForSeconds(rate);
            } while (!isPlayerLost);
        }
    }

    //class EnemyUnit
    //{
    //    public OffensePlaceable enemyType;
    //    public int distance = BattleController.Instance.laneDistance;
    //    public int laneNumber;
    //    public int health;

    //    public EnemyUnit()
    //    {
    //        enemyType = null;
    //        health = 0;
    //    }

    //    public EnemyUnit(OffensePlaceable offensePlaceable, int lane)
    //    {
    //        enemyType = offensePlaceable;
    //        laneNumber = lane;
    //        health = offensePlaceable.health;
    //    }
    //}
}
