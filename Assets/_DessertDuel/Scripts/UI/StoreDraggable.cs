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

        private void Awake()
        {
            canvasRaycaster = StoreController.Instance.mainCanvas.GetComponent<GraphicRaycaster>();
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
                if(result.gameObject.CompareTag("DragReceiver"))
                {
                    tempReceiver = result.gameObject.GetComponent<DragReceiver>();
                }
            }
            if (draggableClicked && tempReceiver != null)
            {
                startDragReceiver = tempReceiver;
                tempReceiver.assignedDraggable = null;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = new Vector3(eventData.position.x, eventData.position.y, 0);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            canvasRaycaster.Raycast(eventData, raycastResults);
            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.CompareTag("DragReceiver"))
                {
                    DragReceiver dragRec = result.gameObject.GetComponent<DragReceiver>();
                    if(dragRec != null && dragRec.assignedDraggable == null)
                    {
                        if(startDragReceiver != null)
                        {
                            startDragReceiver.assignedDraggable = null;
                        }
                        startDragReceiver = null;
                        result.gameObject.GetComponent<DragReceiver>().assignedDraggable = gameObject;
                        transform.position = result.gameObject.transform.position;
                        return;
                    }
                }
            }
            transform.position = startDragPosition;
            if (startDragReceiver != null)
            {
                startDragReceiver.assignedDraggable = gameObject;
            }
            startDragReceiver = null;
        }
    }
}