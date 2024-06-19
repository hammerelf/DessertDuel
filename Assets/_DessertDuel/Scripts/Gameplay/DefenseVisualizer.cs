//Created by: Ryan King

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HammerElf.Games.DessertDuel
{
    [RequireComponent(typeof(DefensePlaceable))]
    public class DefenseVisualizer : MonoBehaviour
    {
        public int health, startHealth, laneNumber;
        public Image image;
        [HideInInspector]
        public DefensePlaceable defenseType;

        public TextMeshProUGUI healthVisualizer, healthStartVisualizer;

        private void Awake()
        {
            defenseType = GetComponent<DefensePlaceable>();
        }

        public void Start()
        {
            startHealth = defenseType.health;
            health = defenseType.health;
            healthStartVisualizer.SetText(startHealth.ToString());
            if (defenseType.itemImage != null)
            {
                image.sprite = defenseType.itemImage.sprite;
                image.color = defenseType.itemImage.color;
            }
        }

        private void Update()
        {
            healthVisualizer.SetText(health.ToString());
        }
    }
}
