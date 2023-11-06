using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupUIHeader : MonoBehaviour, IBeginDragHandler, IDragHandler
{   //헤더부분을 드래그 앤 드롭으로 옮길 수 있게 함
    private RectTransform parentRect;
    private Vector2 rectBegin;
    private Vector2 moveBegin;
    private Vector2 moveOffset;

    private void Awake()
    {
        parentRect = transform.parent.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rectBegin = parentRect.anchoredPosition;
        moveBegin = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        moveOffset = eventData.position - moveBegin;
        parentRect.anchoredPosition = rectBegin + moveOffset;
    }
}
