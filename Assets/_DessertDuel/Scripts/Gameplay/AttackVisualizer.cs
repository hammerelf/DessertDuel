//Created by: Ryan King

using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace HammerElf.Games.DessertDuel
{
    [RequireComponent(typeof(OffensePlaceable))]
    public class AttackVisualizer : MonoBehaviour
    {
        public int distance, health, laneNumber;
        [HideInInspector]
        public OffensePlaceable enemyType;

        public TextMeshProUGUI healthLabel;
        public Slider distanceSlider;
        public Image image;

        private void Awake()
        {
            enemyType = GetComponent<OffensePlaceable>();
        }

        private void Start()
        {
            distance = BattleController.Instance.laneDistance;
            distanceSlider.maxValue = distance;
            image.sprite = enemyType.itemImage.sprite;
            image.color = enemyType.itemImage.color;
        }

        private void Update()
        {
            healthLabel.SetText(health.ToString());
            distanceSlider.value = distance;
        }

        public void SetValues(OffensePlaceable offensePlaceable, int lane)
        {
            enemyType = offensePlaceable;
            laneNumber = lane;
            health = offensePlaceable.health;
            image.sprite = offensePlaceable.itemImage.sprite;
            image.color = offensePlaceable.itemImage.color;
        }
    }
}
