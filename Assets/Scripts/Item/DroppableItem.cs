using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DroppableItem : MonoBehaviour, IDropHandler
{
    public GameObject previousItem;
    public GameObject dropItem;
    public Item tempItem;
    public Image tempImage;
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

            dropItem = dropped;

            Item secondItem = previousItem.GetComponentInParent<Slot>().item;
            //Item firstItem = dropItem.GetComponentInParent<Slot>().item;
            //Image firstImg = dropItem.GetComponentInParent<Slot>().itemIcon;
            Image secondImg = previousItem.GetComponentInParent<Slot>().itemIcon;
            //ChangeItem(firstItem, secondItem, firstImg, secondImg);

        }
        else
        {
            return;
        }
    }

    private void ChangeItem(Item firstItem, Item secondItem, Image firstImg, Image secondImg)
    {
        tempItem = firstItem;
        firstItem = secondItem;
        secondItem = tempItem;

        tempImage = firstImg;
        firstImg = secondImg;
        secondImg = tempImage;
    }
}
