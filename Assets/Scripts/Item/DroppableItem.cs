using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DroppableItem : MonoBehaviour, IDropHandler
{
    GameObject previousItem;
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 1)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentAfterDrag = transform;


            previousItem = transform.GetChild(0).gameObject;
            previousItem.transform.SetParent(draggableItem.parentPreviousDrag);
            previousItem.GetComponent<DraggableItem>().parentPreviousDrag = previousItem.transform.parent.transform;

            draggableItem.parentPreviousDrag = transform;

        }
        else
        {
            return;
        }
    }
}
