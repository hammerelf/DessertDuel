//Created by: Ryan King

using HammerElf.Tools.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HammerElf.Games.DessertDuel
{
    public class StoreDraggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector3 startDragPosition;
        [SerializeField]
        private DragReceiver startDragReceiver;
        private GraphicRaycaster canvasRaycaster;
        private bool isValidationPassed = true;

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
            if (!DragStartValidation(tempReceiver, out string validationMessage))
            {
                ConsoleLog.Log(validationMessage); //Need to show failed validation message to player here.
                tempReceiver.assignedDraggable = null;
                isValidationPassed = false;
                eventData.pointerDrag = null;
                OnEndDrag(eventData);
            }
            if (draggableClicked && tempReceiver != null)
            {
                startDragReceiver = tempReceiver;
                tempReceiver.assignedDraggable = null;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (startDragReceiver != null)
            {
                transform.position = new Vector3(eventData.position.x, eventData.position.y, 0);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isValidationPassed && startDragReceiver != null) //Check if already failed validation on start drag.
            {
                List<RaycastResult> raycastResults = new List<RaycastResult>();
                canvasRaycaster.Raycast(eventData, raycastResults);
                foreach (RaycastResult result in raycastResults)
                {
                    if (result.gameObject.CompareTag("DragReceiver"))
                    {
                        DragReceiver dragRec = result.gameObject.GetComponent<DragReceiver>();
                        ConsoleLog.Log("Attempting to drop draggable on: " + dragRec.name);
                        if (dragRec != null && dragRec.assignedDraggable == null && DragEndValidation(dragRec, out string validationMessage))
                        {
                            startDragReceiver.assignedDraggable = null;
                            startDragReceiver = null;
                            dragRec.assignedDraggable = GetComponent<Placeable>();
                            transform.position = result.gameObject.transform.position;
                            return;
                        }
                    }
                }
            }
            
            //README: may need to separate start drag receiver null logic and drag start validation failed logic and drop failed logic
            
            //Drag failed so reset.
            isValidationPassed = true;
            transform.position = startDragPosition;
            if (startDragReceiver != null)
            {
                startDragReceiver.assignedDraggable = GetComponent<Placeable>();
                startDragReceiver = null;
            }
        }

        public bool DragStartValidation(DragReceiver startReceiver, out string validationMessage)
        {
            validationMessage = "";
            if (startReceiver == null)
            {
                validationMessage = "ERROR: Start receiver null!";
                ConsoleLog.LogError(validationMessage);
                return false;
            }
            bool priceCheck = true;
            if (startReceiver.isShop && !(GameManager.Instance.currentPlayerState.money >= startReceiver.assignedDraggable.cost))
            {
                validationMessage += "Not enough Money!\n";
                priceCheck = false;
            }

            if(priceCheck)
            {
                validationMessage = "Passed";
                return true;
            }
            else
            {
                validationMessage.TrimEnd('\n');
                validationMessage = "Failed: " + validationMessage;
                return false;
            }
        }

        public bool DragEndValidation(DragReceiver dropReceiver, out string validationMessage)
        {
            validationMessage = "";
            if (dropReceiver == null)
            {
                validationMessage = "ERROR: Drop receiver null!";
                ConsoleLog.LogError(validationMessage);
                return false;
            }
            if (dropReceiver == null || dropReceiver.isShop) return false;

            Placeable dragItemToCompare = this.GetComponent<Placeable>();
            bool isOffenseDefense = ((dropReceiver.isDefense && dragItemToCompare.GetType().Equals(typeof(DefensePlaceable))) ||
                    (!dropReceiver.isDefense && dragItemToCompare.GetType().Equals(typeof(OffensePlaceable))));
            
            return isOffenseDefense;
        }
    }
}