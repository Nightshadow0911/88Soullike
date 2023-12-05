using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenUIManager : MonoBehaviour
{
    public static FullScreenUIManager instance;

    [SerializeField] private Image menuIcon;
    [SerializeField] private TMP_Text menuName;
    [SerializeField] private GameObject fullScreenBase;

    public List<FullScreenUI> currentUI; // 현재 켜진 UIList 확인 => 아이콘과 메뉴 이름 변경해주기 위함?

    [Space]
    public KeyCode escapeKey = KeyCode.Escape;
    public KeyCode inventoryKey = KeyCode.I;
    public KeyCode equipmentKey = KeyCode.E;
    public KeyCode charInfoKey = KeyCode.C;
    public KeyCode npcKey = KeyCode.F;
    public KeyCode mapKey = KeyCode.M;

    [Header("UI목록")]
    [SerializeField] private FullScreenUI mainStatusUI;
    [SerializeField] private FullScreenUI basicStatusUI;
    [SerializeField] private FullScreenUI abillityStatusUI;
    [SerializeField] private FullScreenUI playerImageUI;
    [SerializeField] private FullScreenUI inventoryUI;
    [SerializeField] private FullScreenUI itemInformationUI;
    [SerializeField] private FullScreenUI equipmentUI;
    [SerializeField] private FullScreenUI shopUI;
    [SerializeField] private FullScreenUI levelUpUI;
    [SerializeField] private FullScreenUI optionUI;
    [SerializeField] private FullScreenUI mapUI; //
    [SerializeField] private FullScreenUI travelUI; //

    [Header("UI 그룹")] // 여러개의 UI를 넣어두고 0, 1, 2를 처음에 세팅 ... 표시전환 누르면 n번째가 현재 들어와있는것 중 다음거를 부름?
    [SerializeField] private List<FullScreenUI> statusList;
    [SerializeField] private List<FullScreenUI> inventoryList;
    [SerializeField] private List<FullScreenUI> equipmentList;
    [SerializeField] private List<FullScreenUI> shopList;
    [SerializeField] private List<FullScreenUI> levelUpList;
    [SerializeField] private List<FullScreenUI> mapList;

    public LinkedList<FullScreenUI> activeFullScreenUILList;

    // 전체 UI 목록
    private List<FullScreenUI> allFullScreenUIList;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        } 

        Init();
        InitCloseAll();
    }

    private void Update()
    {
        if (Input.GetKeyDown(escapeKey))
        {
            if (activeFullScreenUILList.Count > 0)
            {
                CloseUI(activeFullScreenUILList.First.Value);
            }
            else
            {
                if (optionUI.gameObject.activeSelf)
                {
                    optionUI.gameObject.SetActive(true);
                }
                else
                {
                    optionUI.gameObject.SetActive(true);
                }
            }
        }

        ToggleKeyDownAction(inventoryKey, inventoryList);
        ToggleKeyDownAction(equipmentKey, equipmentList);
        ToggleKeyDownAction(charInfoKey, statusList);
        ToggleKeyDownAction(mapKey, mapList);

        if ((Inventory.instance.currentNPC != null) && (Inventory.instance.currentNPC.isInteractable)) //inventory.instance.currentNPC는 Player.currentNPC로 변경 예정
        {
            if (Inventory.instance.currentNPC.npcName.Equals("Shop"))
            {
                ToggleKeyDownAction(npcKey, shopList);
            }
        }
    }

    private void Init()
    {
        allFullScreenUIList = new List<FullScreenUI>() // 리스트 초기화
        {
            mainStatusUI, basicStatusUI, abillityStatusUI, playerImageUI, inventoryUI, itemInformationUI, equipmentUI, shopUI, levelUpUI
        };
        statusList = new List<FullScreenUI>() {mainStatusUI, basicStatusUI, playerImageUI, abillityStatusUI };
        inventoryList = new List<FullScreenUI>() { inventoryUI, itemInformationUI, mainStatusUI, basicStatusUI, abillityStatusUI };
        equipmentList = new List<FullScreenUI>() { equipmentUI, itemInformationUI, mainStatusUI, basicStatusUI, abillityStatusUI };
        shopList = new List<FullScreenUI>() {shopUI, itemInformationUI, mainStatusUI, basicStatusUI, abillityStatusUI };
        levelUpList = new List<FullScreenUI>() { levelUpUI, basicStatusUI, abillityStatusUI };
        mapList = new List<FullScreenUI>() { mapUI, travelUI};

    }

    private void InitCloseAll() // 시작시 모든 팝업 닫기
    {
        foreach (FullScreenUI fScreen in allFullScreenUIList)
        {
            CloseUI(fScreen);
        }
    }

    // 단축키 입력에 따라 팝업 열거나 닫기
    private void ToggleKeyDownAction(in KeyCode key, List<FullScreenUI> fScreens)
    {
        if (Input.GetKeyDown(key))
        {
            ToggleOpenCloseUIList(fScreens);
        }
    }

    // 팝업의 상태에 따라 열거나 닫기(opened / closed)
    private void ToggleOpenCloseUI(FullScreenUI fScreen)
    {
        if (!fScreen.gameObject.activeSelf)
        {
            OpenUI(fScreen);
        }
        else
        {
            CloseUI(fScreen);
        }
    }

    private void ToggleOpenCloseUIList(List<FullScreenUI> fScreens)
    {
        if (fScreens == null) return;

        foreach(FullScreenUI fScreen in fScreens)
        {
            if (!fScreen.gameObject.activeSelf)
            {
                OpenUI(fScreen);
            }
            else
            {
                CloseUI(fScreen);
            }
        }
    }

    // 팝업을 열고 링크드리스트의 상단에 추가
    private void OpenUI(FullScreenUI fScreen)
    {
        fullScreenBase.SetActive(true);
        activeFullScreenUILList.AddFirst(fScreen);
        fScreen.gameObject.SetActive(true);
        RefreshAllPopupDepth();
    }
    private void OpenUIList(List<FullScreenUI> fScreens)
    {
        if (fScreens == null) return;

        fullScreenBase.SetActive(true);
        foreach (FullScreenUI fScreen in fScreens)
        {
            activeFullScreenUILList.AddFirst(fScreen);
            fScreen.gameObject.SetActive(true);
        }
        RefreshAllPopupDepth();
    }

    // 팝업을 닫고 링크드리스트에서 제거
    private void CloseUI(FullScreenUI fScreen)
    {
        fullScreenBase.SetActive(false);
        activeFullScreenUILList.Remove(fScreen);
        fScreen.gameObject.SetActive(false);
        RefreshAllPopupDepth();
    }
    private void CloseUIList(List<FullScreenUI> fScreens)
    {
        if (fScreens == null) return;

        fullScreenBase.SetActive(false);
        foreach (FullScreenUI fScreen in fScreens)
        {
            activeFullScreenUILList.Remove(fScreen);
            fScreen.gameObject.SetActive(false);
        }
        RefreshAllPopupDepth();
    }

    //링크드리스트 내 모든 팝업의 자식 순서 재배치
    private void RefreshAllPopupDepth()
    {
        foreach (FullScreenUI fScreen in activeFullScreenUILList)
        {
            fScreen.transform.SetAsFirstSibling();
        }
    }

}