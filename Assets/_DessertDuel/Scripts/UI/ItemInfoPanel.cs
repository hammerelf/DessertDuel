//Created by: Ryan King

using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HammerElf.Games.DessertDuel
{
    public class ItemInfoPanel : MonoBehaviour
    {
        [SerializeField, TitleGroup("Base")]
        private Image itemImageValue;
        [SerializeField, TitleGroup("Base")]
        private TextMeshProUGUI offDefValue, nameValue, costValue, powerValue, descriptionValue;

        //Shared
        [SerializeField, TitleGroup("Base")]
        private TextMeshProUGUI healthValue;

        [SerializeField, TitleGroup("Defense")]
        private GameObject damageLabel, attackRateLabel;
        [SerializeField, TitleGroup("Defense")]
        private TextMeshProUGUI damageValue, attackRateValue;

        [SerializeField, TitleGroup("Offense")]
        private GameObject moveSpeedLabel, spawnRateLabel;
        [SerializeField, TitleGroup("Offense")]
        private TextMeshProUGUI moveSpeedValue, spawnRateValue;

        //Eventually will need to switch this over to layout group that adds and removes 
        //offense/defense specific fields.
        public void SetValues(Placeable placeable)
        {
            if (placeable == null) return;

            itemImageValue.sprite = placeable.itemImage.sprite;
            //Need to do this for now since my images are all just color changes.
            itemImageValue.color = placeable.itemImage.color;
            nameValue.text = placeable.itemName;
            costValue.text = placeable.cost.ToString();
            powerValue.text = placeable.power.ToString();
            descriptionValue.text = placeable.description;

            if(placeable.GetType() == typeof(DefensePlaceable))
            {
                //Size currently hard-coded until I do more with offense/defense icons.
                itemImageValue.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
                itemImageValue.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);

                moveSpeedLabel.SetActive(false);
                spawnRateLabel.SetActive(false);
                moveSpeedValue.gameObject.SetActive(false);
                spawnRateValue.gameObject.SetActive(false);

                damageLabel.SetActive(true);
                attackRateLabel.SetActive(true);
                damageValue.gameObject.SetActive(true);
                attackRateValue.gameObject.SetActive(true);

                DefensePlaceable dPlaceable = placeable as DefensePlaceable;

                offDefValue.text = "Defense";
                healthValue.text = dPlaceable.health.ToString();
                damageValue.text = dPlaceable.damage.ToString();
                attackRateValue.text = dPlaceable.attackRate.ToString();
            }
            else
            {
                //Size currently hard-coded until I do more with offense/defense icons.
                itemImageValue.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 50);
                itemImageValue.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);

                moveSpeedLabel.SetActive(true);
                spawnRateLabel.SetActive(true);
                moveSpeedValue.gameObject.SetActive(true);
                spawnRateValue.gameObject.SetActive(true);

                damageLabel.SetActive(false);
                attackRateLabel.SetActive(false);
                damageValue.gameObject.SetActive(false);
                attackRateValue.gameObject.SetActive(false);

                OffensePlaceable oPlaceable = placeable as OffensePlaceable;
                
                offDefValue.text = "Offense";
                healthValue.text = oPlaceable.health.ToString();
                moveSpeedValue.text = oPlaceable.moveSpeed.ToString();
                spawnRateValue.text = oPlaceable.spawnRate.ToString();
            }
        }
    }
}
