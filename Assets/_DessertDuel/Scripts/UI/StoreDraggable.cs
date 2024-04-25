//Created by: Ryan King

using HammerElf.Tools.Utilities;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HammerElf.Games.DessertDuel
{
    public class StoreDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector3 startDragPosition;
        private GraphicRaycaster canvasRaycaster;
        private bool isValidationPassed = true;
        [Space, Space]
        [SerializeField, ReadOnly, FoldoutGroup("VariableInfo", expanded: false)]
        private string dragValidationMessage, dropValidationMessage, purchaseValidationMessage;
        [SerializeField, ReadOnly, FoldoutGroup("VariableInfo")]
        private DragReceiver startDragReceiver;

        private void Awake()
        {
            canvasRaycaster = StoreController.Instance.mainCanvas.GetComponent<GraphicRaycaster>();
            startDragPosition = transform.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            canvasRaycaster.Raycast(eventData, raycastResults);
            bool draggableClicked = false;
            DragReceiver tempReceiver = null;
            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.CompareTag("Draggable"))
                {
                    startDragPosition = transform.position;
                    draggableClicked = true;
                    continue;
                }
                if (result.gameObject.CompareTag("DragReceiver"))
                {
                    tempReceiver = result.gameObject.GetComponent<DragReceiver>();
                }
            }
            //Short-circuit drag event if validation fails.
            if (!DragStartValidation(tempReceiver, out dragValidationMessage))
            {
                ConsoleLog.Log(dragValidationMessage); //Need to show failed validation message to player here.
                isValidationPassed = false;
                eventData.pointerDrag = null;
                OnEndDrag(eventData);
                return;
            }
            ConsoleLog.Log(dragValidationMessage); //Need to show failed validation message to player here.
            if (draggableClicked && tempReceiver != null)
            {
                startDragReceiver = tempReceiver;
            }
        }

        //TODO: Need to make sure current item being dragged is on top of other items until dropped.
        public void OnDrag(PointerEventData eventData)
        {
            if (startDragReceiver != null)
            {
                transform.position = new Vector3(eventData.position.x, eventData.position.y, 0);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isValidationPassed) //Check if already failed validation on start drag.
            {
                if (startDragReceiver != null)
                {
                    List<RaycastResult> raycastResults = new List<RaycastResult>();
                    canvasRaycaster.Raycast(eventData, raycastResults);
                    foreach (RaycastResult result in raycastResults)
                    {
                        if (result.gameObject.CompareTag("DragReceiver"))
                        {
                            DragReceiver dragRec = result.gameObject.GetComponent<DragReceiver>();

                            if (dragRec != null && dragRec.assignedDraggable == null && DragEndValidation(dragRec, out dropValidationMessage))
                            {
                                if (TryPurchase(startDragReceiver, out purchaseValidationMessage))
                                {
                                    startDragReceiver.assignedDraggable = null;
                                    startDragReceiver = null;
                                    dragRec.assignedDraggable = GetComponent<Placeable>();
                                    transform.position = result.gameObject.transform.position;
                                    isValidationPassed = true;
                                    return;
                                }
                                ConsoleLog.Log(purchaseValidationMessage); //Need to show failed validation message to player here.
                            }
                            ConsoleLog.Log(dropValidationMessage); //Need to show failed validation message to player here.
                        }
                    }
                }

                //Drag failed so reset.
                transform.position = startDragPosition;
                if (startDragReceiver != null)
                {
                    startDragReceiver.assignedDraggable = GetComponent<Placeable>();
                    startDragReceiver = null;
                }
            }
            isValidationPassed = true;
        }

        private bool DragStartValidation(DragReceiver startReceiver, out string validationMessage)
        {
            validationMessage = "";
            if (startReceiver == null)
            {
                validationMessage = "Error: Start receiver null!";
                ConsoleLog.LogError(validationMessage);
                return false;
            }

            bool poorCheck = true;
            if (startReceiver.isShop && GameManager.Instance.currentPlayerState.money <= 0)
            {
                validationMessage += "Money is 0.\n";
                poorCheck = false;
            }

            if(poorCheck) //Check each validation bool here.
            {
                validationMessage = "Passed.";
                return true;
            }
            else
            {
                validationMessage.TrimEnd('\n');
                validationMessage = "Failed: " + validationMessage;
                return false;
            }
        }

        private bool DragEndValidation(DragReceiver dropReceiver, out string validationMessage)
        {
            validationMessage = "";
            if (dropReceiver == null)
            {
                validationMessage = "Drop receiver null!";
                ConsoleLog.LogError(validationMessage);
                return false;
            }

            bool shopCheck = true;
            if (dropReceiver.isShop)
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
                return true;
            }
            else
            {
                validationMessage.TrimEnd('\n');
                validationMessage = "Failed: " + validationMessage;
                return false;
            }
        }

        //Check if from store, if not return. If it is then check how much the draggable costs then 
        //check how much money the player has. If the player has enough money subtract that much money 
        //and return true. If the player doesn't have enough money return false and a purchase check 
        //failed message.
        private bool TryPurchase(DragReceiver startReciever, out string validationMessage)
        {
            validationMessage = "";
            if (!startReciever.isShop) // Not moving from a shop reciever so don't need to check.
            {
                validationMessage = "Passed: Not a store item.";
                return true;
            }

            if (startReciever.assignedDraggable.cost <= GameManager.Instance.currentPlayerState.money)
            {
                validationMessage = "Passed: Making purchase.";
                GameManager.Instance.currentPlayerState.money -= startReciever.assignedDraggable.cost;
                return true;
            }
            validationMessage = "Failed: Not enough money.";
            return false;
        }
    }
}