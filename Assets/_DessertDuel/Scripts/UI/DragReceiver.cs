//Created by: Ryan King

using HammerElf.Tools.Utilities;
using UnityEngine;

namespace HammerElf.Games.DessertDuel
{
    public class DragReceiver : MonoBehaviour
    {
        public Placeable assignedDraggable;
        public PlaceableState slotState;
        public bool isDefense;

        public void PositionPlaceable()
        {
            if (assignedDraggable == null) ConsoleLog.Log("Assigned draggable null for: " + gameObject.name);
            assignedDraggable.transform.position = transform.position;
        }
    }
}
