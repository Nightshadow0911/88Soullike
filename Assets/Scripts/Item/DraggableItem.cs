using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public Item parentItem;
    public int parentSlotnum;
    [HideInInspector] public Transform parentAfterDrag;
    public Transform parentPreviousDrag; // slot���� �ڱ� �ڽ����� ����

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");
        parentAfterDrag = transform.parent;
        
        parentItem = parentPreviousDrag.GetComponent<Slot>().item;
        parentSlotnum = parentPreviousDrag.GetComponent<Slot>().slotnum;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }

}
