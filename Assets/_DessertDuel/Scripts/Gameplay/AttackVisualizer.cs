//Created by: Ryan King

using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace HammerElf.Games.DessertDuel
{
    [RequireComponent(typeof(OffensePlaceable))]
    public class AttackVisualizer : MonoBehaviour
    {
        public int distance;
        public int health;
        public int laneNumber;
        [HideInInspector]
        public OffensePlaceable enemyType;

        public TextMeshProUGUI healthLabel;
        public Slider distanceSlider;


        private void Awake()
        {
            enemyType = GetComponent<OffensePlaceable>();
        }

        private void Start()
        {
            distance = BattleController.Instance.laneDistance;
            distanceSlider.maxValue = distance;
        }

        private void Update()
        {
            healthLabel.text = enemyType.health.ToString();
            distanceSlider.value = distance;
        }

        public void SetValues(OffensePlaceable offensePlaceable, int lane)
        {
            enemyType = offensePlaceable;
            laneNumber = lane;
            health = offensePlaceable.health;
        }
    }
}
