using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class TravelPoint : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 position;
    private Color changeColor;
    private bool isReady;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private Image imageUI;


    private void Awake()
    {
        changeColor = imageUI.color;
        CloseText();
    }

    public void SetTravel(string name, Vector3 position)
    {
        this.position = position + (Vector3.left * 2);
        textUI.text = name;
        gameObject.SetActive(true);
    }

    public void ChangeReady(bool state)
    {
        changeColor.a = state ? 1f : 0.3f;
        imageUI.color = changeColor;
        isReady = state;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isReady)
        {
            GameManager.instance.player.transform.position = position;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textUI.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CloseText();
    }

    public void CloseText()
    {
        textUI.gameObject.SetActive(false);
    }
}
