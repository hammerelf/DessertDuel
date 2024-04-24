//Created by: Ryan King

using HammerElf.Tools.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace HammerElf.Games.DessertDuel
{
    public class StoreController : Singleton<StoreController>
    {
        public Canvas mainCanvas;
        public Transform playerDraggableHolder;
        [SerializeField]
        private Transform storeDraggableHolder;
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
            if (storeDraggableHolder.childCount > 0)
            {
                for (int i = 0; i < storeDraggableHolder.childCount; i++)
                {
                    Destroy(storeDraggableHolder.GetChild(i).gameObject);
                }
            }
            foreach(DragReceiver storeSlot in defenseStoreSlots)
            {
                if(defenseOptions != null && defenseOptions.Count > 0)
                {
                    GameObject go = Instantiate(defenseOptions[Random.Range(0, defenseOptions.Count)], storeDraggableHolder);
                    storeSlot.assignedDraggable = go.GetComponent<Placeable>();
                    if (storeSlot.assignedDraggable == null) ConsoleLog.Log("Assigned draggable is null. Slot name: " + storeSlot.gameObject.name);
                    storeSlot.PositionPlaceable();
                }
            }
            foreach(DragReceiver storeSlot in offenseStoreSlots)
            {
                if(offenseOptions != null && offenseOptions.Count > 0)
                {
                    GameObject go = Instantiate(offenseOptions[Random.Range(0, offenseOptions.Count)], storeDraggableHolder);
                    storeSlot.assignedDraggable = go.GetComponent<Placeable>();
                    storeSlot.PositionPlaceable();
                }
            }
        }
    }
}
