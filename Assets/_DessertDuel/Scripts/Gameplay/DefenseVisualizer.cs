//Created by: Ryan King

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HammerElf.Games.DessertDuel
{
    [RequireComponent(typeof(DefensePlaceable))]
    public class DefenseVisualizer : MonoBehaviour
    {
        public int health, startHealth;
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
            healthStartVisualizer.text = startHealth.ToString();
            image = defenseType.itemImage;
        }

        private void Update()
        {
            health = defenseType.health;
            healthVisualizer.text = health.ToString();
        }
    }
}
