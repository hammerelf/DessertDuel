//Created by: Ryan King

using HammerElf.Tools.Utilities;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HammerElf.Games.DessertDuel
{
    public class StoreController : Singleton<StoreController>
    {
        public GraphicRaycaster graphicRaycaster;
        public ItemInfoPanel itemInfoPanel;
        public TextMeshProUGUI dragValidationOutput, dropValidationOutput, purchaseValidationOutput;

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
        [SerializeField]
        private int rerollCost = 1;

        [SerializeField]
        private DragReceiver defenseSlot1, defenseSlot2, defenseSlot3;
        [SerializeField]
        private List<DragReceiver> offenseSlots1 = new(3), offenseSlots2 = new(3), offenseSlots3 = new(3);
        [SerializeField]
        private List<DragReceiver> offenseStorage, defenseStorage;

        protected override void Awake()
        {
            base.Awake();
            GameManager.Instance.storeController = this;
        }

        private void Start()
        {
            LoadStore();
        }

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

        //If there is enough money to reroll then subtract rerollCost from money and call LoadStore.
        //If not then play "Not enough money" feedback.
        public void RerollStore()
        {
            if(GameManager.Instance.currentPlayerState.money >= rerollCost)
            {
                GameManager.Instance.currentPlayerState.money -= rerollCost;
                LoadStore();
            }
            else
            {
                //Not enough money feedback will go here.
                ConsoleLog.Log("Not enough money to reroll.");
            }
        }

        //Saves currently placed items to json so that they can be accessed later or in a
        //different scene. Must iterate through each defense and offense placement and
        //save the id which can then be looked up later to place items again.
        public void SaveStoreState()
        {
            JsonPlayerState placementHolder = GameManager.Instance.jPlayerState;

            placementHolder.defensePlacement1 = defenseSlot1.assignedDraggable != null ? defenseSlot1.assignedDraggable.id : "";
            placementHolder.defensePlacement2 = defenseSlot2.assignedDraggable != null ? defenseSlot2.assignedDraggable.id : "";
            placementHolder.defensePlacement3 = defenseSlot3.assignedDraggable != null ? defenseSlot3.assignedDraggable.id : "";

            for (int i = 0; i < 3; i++)
            {
                placementHolder.offensePlacements1[i] = offenseSlots1[i].assignedDraggable != null ? offenseSlots1[i].assignedDraggable.id : "";
                placementHolder.offensePlacements2[i] = offenseSlots2[i].assignedDraggable != null ? offenseSlots2[i].assignedDraggable.id : "";
                placementHolder.offensePlacements3[i] = offenseSlots3[i].assignedDraggable != null ? offenseSlots3[i].assignedDraggable.id : "";
            }

            placementHolder.offenseStore.Clear();
            foreach (DragReceiver oStoreSlot in offenseStoreSlots)
            {
                if (oStoreSlot.assignedDraggable != null)
                {
                    placementHolder.offenseStore.Add(oStoreSlot.assignedDraggable.id);
                }
            }

            placementHolder.defenseStore.Clear();
            foreach (DragReceiver dStoreSlot in defenseStoreSlots)
            {
                if (dStoreSlot.assignedDraggable != null)
                {
                    placementHolder.defenseStore.Add(dStoreSlot.assignedDraggable.id);
                }
            }

            placementHolder.offenseStorage.Clear();
            foreach(DragReceiver oStorageSlot in offenseStorage)
            {
                if(oStorageSlot.assignedDraggable != null)
                {
                    placementHolder.offenseStorage.Add(oStorageSlot.assignedDraggable.id);
                }    
            }

            placementHolder.defenseStorage.Clear();
            foreach(DragReceiver dStorageSlot in defenseStorage)
            {
                if(dStorageSlot.assignedDraggable != null)
                {
                    placementHolder.defenseStorage.Add(dStorageSlot.assignedDraggable.id);
                }
            }

            placementHolder.SaveJson(placementHolder.PlayerStateToJson());
        }
    }
}
