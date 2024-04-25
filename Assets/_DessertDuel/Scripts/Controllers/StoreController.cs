//Created by: Ryan King

using HammerElf.Tools.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace HammerElf.Games.DessertDuel
{
    public class StoreController : Singleton<StoreController>
    {
        public Canvas mainCanvas;
        [SerializeField]
        private Transform draggableHolder;
        [SerializeField]
        private List<GameObject> defenseOptions;
        [SerializeField]
        private List<GameObject> offenseOptions;
        [SerializeField]
        private List<DragReceiver> defenseStoreSlots;
        [SerializeField]
        private List<DragReceiver> offenseStoreSlots;

        public void LoadStore()
        {
            if (draggableHolder.childCount > 0)
            {
                for (int i = 0; i < draggableHolder.childCount; i++)
                {
                    if(draggableHolder.GetChild(i).GetComponent<Placeable>().state.Equals(PlaceableState.SHOP))
                    {
                        Destroy(draggableHolder.GetChild(i).gameObject);
                    }
                }
            }
            foreach(DragReceiver storeSlot in defenseStoreSlots)
            {
                if(defenseOptions != null && defenseOptions.Count > 0)
                {
                    GameObject go = Instantiate(defenseOptions[Random.Range(0, defenseOptions.Count)], draggableHolder);
                    SetUpStoreItem(storeSlot, go);
                }
            }
            foreach(DragReceiver storeSlot in offenseStoreSlots)
            {
                if(offenseOptions != null && offenseOptions.Count > 0)
                {
                    GameObject go = Instantiate(offenseOptions[Random.Range(0, offenseOptions.Count)], draggableHolder);
                    SetUpStoreItem(storeSlot, go);
                }
            }
        }

        private void SetUpStoreItem(DragReceiver storeReceiver, GameObject storeItemObject)
        {
            Placeable placeable = storeItemObject.GetComponent<Placeable>();
            placeable.state = PlaceableState.SHOP;
            storeReceiver.assignedDraggable = placeable;
            if (storeReceiver.assignedDraggable == null) ConsoleLog.Log("Assigned draggable is null. Slot name: " + storeReceiver.gameObject.name);
            storeReceiver.PositionPlaceable();
        }
    }
}
