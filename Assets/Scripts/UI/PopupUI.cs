using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class PopupUI : MonoBehaviour, IPointerDownHandler
{
    public Button closeButton;
    public event Action OnFocus;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnFocus();
    }
}
