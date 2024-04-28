//Created by: Ryan King

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HammerElf.Games.DessertDuel
{
    public class HoverInfo : MonoBehaviour, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            StoreController.Instance.graphicRaycaster.Raycast(eventData, raycastResults);
            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.CompareTag("Placeable"))
                {
                    StoreController.Instance.itemInfoPanel.SetValues(result.gameObject.GetComponent<Placeable>());
                    return;
                }
            }
        }
    }
}
