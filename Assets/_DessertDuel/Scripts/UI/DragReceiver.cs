//Created by: Ryan King

using HammerElf.Tools.Utilities;
using UnityEngine;

namespace HammerElf.Games.DessertDuel
{
    public class DragReceiver : MonoBehaviour
    {
        public Placeable assignedDraggable;
        public bool isActive;
        public bool isDefense;
        public bool isShop;

        public void PositionPlaceable()
        {
            if (assignedDraggable == null) ConsoleLog.Log("Assigned draggable null for: " + gameObject.name);
            assignedDraggable.transform.position = transform.position;
        }
    }
}
