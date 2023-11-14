using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : MonoBehaviour
{
    public static PopupUIManager instance;
    
    public PopupUI inventoryPopup;
    public PopupUI equipmentPopup;
    public PopupUI characterInfoPopup;
    public PopupUI shopPopup;
    public PopupUI usePopup;
    public PopupUI mapPopup;
    public GameObject optionPopup;
    

    [Space]
    public KeyCode escapeKey = KeyCode.Escape;
    public KeyCode inventoryKey = KeyCode.I;
    public KeyCode equipmentKey = KeyCode.E;
    public KeyCode charInfoKey = KeyCode.C;
    public KeyCode npcKey = KeyCode.X;
    public KeyCode mapKey = KeyCode.M;

    // 실시간 팝업 관리 링크드 리스트
    public LinkedList<PopupUI> activePopupLList;

    // 전체 팝업 목록
    private List<PopupUI> allPopupList;

    private void Awake()
    {
        #region Singleton
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        #endregion

        activePopupLList = new LinkedList<PopupUI>();
        Init();
        InitCloseAll();
    }

    private void Update()
    {
        if(Input.GetKeyDown(escapeKey))
        {
            if(activePopupLList.Count > 0)
            {
                ClosePopup(activePopupLList.First.Value);
            } else
            {
                if(optionPopup.activeSelf)
                {
                    optionPopup.SetActive(false);
                }
                else
                {
                    optionPopup.SetActive(true);
                }
            }
        }

        ToggleKeyDownAction(inventoryKey, inventoryPopup);
        ToggleKeyDownAction(equipmentKey, equipmentPopup);
        ToggleKeyDownAction(charInfoKey, characterInfoPopup);
        ToggleKeyDownAction(mapKey, mapPopup);

        if ((Inventory.instance.currentNPC != null) && (Inventory.instance.currentNPC.isInteractable)) //inventory.instance.currentNPC는 Player.currentNPC로 변경 예정
        {
            if(Inventory.instance.currentNPC.npcName.Equals("Shop"))
            {
                ToggleKeyDownAction(npcKey, shopPopup);
            }
            if(Inventory.instance.currentNPC.npcName.Equals("Stat"))
            {
                CharacterInfoUI.instance.growPopupBtn.SetActive(true);
            }
        }
    }

    private void Init()
    {
        allPopupList = new List<PopupUI>() // 리스트 초기화
        {
            inventoryPopup, equipmentPopup, characterInfoPopup, mapPopup, shopPopup, usePopup
        };

        foreach (PopupUI popup in allPopupList) // 모든 팝업에 이벤트 등록
        {
            popup.OnFocus += () => //헤더 포커스 이벤트
            { 
                activePopupLList.Remove(popup);
                activePopupLList.AddFirst(popup);
                RefreshAllPopupDepth();
            };

            popup.closeButton.onClick.AddListener(() => ClosePopup(popup)); // 닫기버튼 이벤트
        }
    }

    private void InitCloseAll() // 시작시 모든 팝업 닫기
    {
        foreach (PopupUI popup in allPopupList)
        {
            ClosePopup(popup);
        }
    }

    // 단축키 입력에 따라 팝업 열거나 닫기
    private void ToggleKeyDownAction(in KeyCode key, PopupUI popup)
    {
        if(Input.GetKeyDown(key))
        {
            ToggleOpenClosePopup(popup);
        }
    }

    // 팝업의 상태에 따라 열거나 닫기(opened / closed)
    private void ToggleOpenClosePopup(PopupUI popup)
    {
        if(!popup.gameObject.activeSelf)
        {
            OpenPopup(popup);
        } else
        {
            ClosePopup(popup);
        }
    }

    // 팝업을 열고 링크드리스트의 상단에 추가
    private void OpenPopup(PopupUI popup)
    {
        activePopupLList.AddFirst(popup);
        popup.gameObject.SetActive(true);
        RefreshAllPopupDepth();
    }

    // 팝업을 닫고 링크드리스트에서 제거
    private void ClosePopup(PopupUI popup)
    {
        activePopupLList.Remove(popup);
        popup.gameObject.SetActive(false);
        RefreshAllPopupDepth();
    }

    //링크드리스트 내 모든 팝업의 자식 순서 재배치
    private void RefreshAllPopupDepth()
    {
        foreach(PopupUI popup in activePopupLList)
        {
            popup.transform.SetAsFirstSibling();
        }
    }

}
