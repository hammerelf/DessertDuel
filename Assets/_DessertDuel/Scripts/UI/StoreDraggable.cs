//Created by: Ryan King

using HammerElf.Tools.Utilities;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HammerElf.Games.DessertDuel
{
    public class StoreDraggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private Vector3 startDragPosition;
        //private bool isDragValidationPassed = true;
        private Placeable thisPlaceable;
        [Space, Space]
        [SerializeField, ReadOnly, FoldoutGroup("VariableInfo", expanded: false)]
        private string dragValidationMessage, dropValidationMessage, purchaseValidationMessage;
        [SerializeField, ReadOnly, FoldoutGroup("VariableInfo")]
        private DragReceiver startDragReceiver;

        private void Awake()
        {
            startDragPosition = transform.position;
            thisPlaceable = GetComponent<Placeable>();
        }

        //Drag placeable if startDragReceiver is not null, which is set in OnPointerDown.
        private void Update()
        {
            if (startDragReceiver != null)
            {
                transform.position = Input.mousePosition;
            }
        }

        //If DragReceiver is clicked on and start drag logic passes, then set startDragReceiver.
        public void OnPointerDown(PointerEventData eventData)
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            StoreController.Instance.graphicRaycaster.Raycast(eventData, raycastResults);
            DragReceiver tempReceiver = null;

            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.CompareTag("DragReceiver"))
                {
                    tempReceiver = result.gameObject.GetComponent<DragReceiver>();
                }
            }

            //If start validation fails, startDragReceiver remains null which disables move and drop logic.
            if(tempReceiver != null && DragStartValidation(tempReceiver, out dragValidationMessage))
            {
                startDragReceiver = tempReceiver;
                transform.SetAsLastSibling();
            }
        }

        //Handle placement validation, buying, and selling when placeable being dragged is dropped on a receiver.
        public void OnPointerUp(PointerEventData eventData)
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            StoreController.Instance.graphicRaycaster.Raycast(eventData, raycastResults);
            DragReceiver dragReceiverDestination;
            foreach (RaycastResult result in raycastResults)
            {
                if (startDragReceiver != null)
                {
                    if (!result.gameObject.name.Equals("SellSlot"))
                    {
                        if (result.gameObject.CompareTag("DragReceiver"))
                        {
                            dragReceiverDestination = result.gameObject.GetComponent<DragReceiver>();

                            if (dragReceiverDestination != null && dragReceiverDestination.assignedDraggable == null && DragEndValidation(dragReceiverDestination, out dropValidationMessage))
                            {
                                if (TryPurchase(startDragReceiver, out purchaseValidationMessage))
                                {
                                    thisPlaceable.state = dragReceiverDestination.slotState;
                                    startDragReceiver.assignedDraggable = null;
                                    startDragReceiver = null;
                                    dragReceiverDestination.assignedDraggable = thisPlaceable;
                                    transform.position = result.gameObject.transform.position;
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (startDragReceiver.assignedDraggable.state != PlaceableState.SHOP ||
                            startDragReceiver.assignedDraggable.state != PlaceableState.NONE)
                        {
                            GameManager.Instance.currentPlayerState.money += startDragReceiver.assignedDraggable.cost / 2;
                            Destroy(gameObject);
                            return;
                        }
                    }
                }
            }

            //Drag failed so reset.
            if (startDragReceiver != null)
            {
                thisPlaceable.state = startDragReceiver.slotState;
                startDragReceiver.PositionPlaceable();
                startDragReceiver = null;
            }
        }

        //When placeable is picked up validate the following: 
        //      -If starting at store, does the player have any money (may need to be removed for free items later).
        private bool DragStartValidation(DragReceiver startReceiver, out string validationMessage)
        {
            validationMessage = "";

            //This shouldn't be happening but if it does the logic should handle it without breaking.
            if (startReceiver == null)
            {
                validationMessage = "Error: Start receiver null!";
                ConsoleLog.LogWarning(validationMessage);
                return false;
            }

            if (startReceiver.assignedDraggable == null)
            {
                validationMessage = "Empty receivable slot.";
                StoreController.Instance.dragValidationOutput.text = dragValidationMessage;
                return false;
            }

            bool poorCheck = true;
            if (startReceiver.slotState.Equals(PlaceableState.SHOP) && GameManager.Instance.currentPlayerState.money <= 0)
            {
                validationMessage += "Money is 0.\n";
                poorCheck = false;
            }

            if(poorCheck) //Check each validation bool here.
            {
                validationMessage = "Passed.";
                StoreController.Instance.dragValidationOutput.text = dragValidationMessage;
                return true;
            }
            else
            {
                validationMessage.TrimEnd('\n');
                validationMessage = "Failed: " + validationMessage;
                StoreController.Instance.dragValidationOutput.text = dragValidationMessage;
                return false;
            }
        }

        //When placeable is dropped validate the following: 
        //      -Not dropping into shop.
        //      -Not dropping offense/defense into the wrong type of receiver.
        private bool DragEndValidation(DragReceiver dropReceiver, out string validationMessage)
        {
            validationMessage = "";
            //This shouldn't be happening but if it does the logic should handle it without breaking.
            if (dropReceiver == null)
            {
                validationMessage = "Drop receiver null!";
                ConsoleLog.LogWarning(validationMessage);
                return false;
            }

            bool shopCheck = true;
            if (dropReceiver.slotState.Equals(PlaceableState.SHOP))
            {
                validationMessage += "Destination is shop.\n";
                shopCheck = false;
            }

            Placeable dragItemToCompare = this.GetComponent<Placeable>();
            bool isOffenseDefense = true;
            if((!dropReceiver.isDefense && dragItemToCompare.GetType().Equals(typeof(DefensePlaceable))) ||
                    (dropReceiver.isDefense && dragItemToCompare.GetType().Equals(typeof(OffensePlaceable))))
            {
                validationMessage += "Wrong type of placeable.\n";
                isOffenseDefense = false;
            }

            if (isOffenseDefense && shopCheck) //Check each validation bool here.
            {
                validationMessage = "Passed.";
                StoreController.Instance.dropValidationOutput.text = dragValidationMessage;
                return true;
            }
            else
            {
                validationMessage.TrimEnd('\n');
                validationMessage = "Failed: " + validationMessage;
                StoreController.Instance.dropValidationOutput.text = dragValidationMessage;
                return false;
            }
        }

        //Check if from store, if not return. If it is then check how much the draggable costs then 
        //check how much money the player has. If the player has enough money subtract that much money 
        //and return true. If the player doesn't have enough money return false and a purchase check 
        //failed message.
        private bool TryPurchase(DragReceiver startReceiver, out string validationMessage)
        {
            validationMessage = "";
            if (!startReceiver.slotState.Equals(PlaceableState.SHOP)) // Not moving from a shop receiver so don't need to check.
            {
                validationMessage = "Passed: Not a store item.";
                return true;
            }
            if (startReceiver.assignedDraggable.cost <= GameManager.Instance.currentPlayerState.money)
            {
                validationMessage = "Passed: Making purchase.";
                StoreController.Instance.purchaseValidationOutput.text = dragValidationMessage;
                GameManager.Instance.currentPlayerState.money -= startReceiver.assignedDraggable.cost;
                ConsoleLog.Log("Player purchased a " + startReceiver.assignedDraggable.name +
                               "\n" + startReceiver.assignedDraggable.ToString());
                return true;
            }
            else
            {
                validationMessage = "Failed: Not enough money.";
                StoreController.Instance.purchaseValidationOutput.text = dragValidationMessage;
                return false;
            }
        }
    }
}