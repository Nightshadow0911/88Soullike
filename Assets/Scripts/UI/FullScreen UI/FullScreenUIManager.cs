using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenUIManager : MonoBehaviour
{
    public static FullScreenUIManager instance;

    [SerializeField] private Image menuIcon;
    [SerializeField] private List<Sprite> menuIconList;
    [SerializeField] private TMP_Text menuName;
    [SerializeField] private GameObject fullScreenBase;

    public List<FullScreenUI> currentUI; // 현재 켜진 UIList 확인 => 아이콘과 메뉴 이름 변경해주기 위함?
    public int uiIndex;

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

    [SerializeField] private LinkedList<FullScreenUI> activeFullScreenUILList;

    // 전체 UI 목록
    [SerializeField] private List<FullScreenUI> allFullScreenUIList;
    [SerializeField] private Vector2 leftPanelPosition;
    [SerializeField] private Vector2 centerPanelPosition;
    [SerializeField] private Vector2 rightPanelPosition;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        activeFullScreenUILList = new LinkedList<FullScreenUI>();
        //menuIconList = new List<Sprite>(); Resources/,...

        Init();
        InitCloseAll();
    }

    private void Update()
    {
        if (Input.GetKeyDown(escapeKey))
        {
            if (activeFullScreenUILList.Count > 0)
            {
                CloseUIList(allFullScreenUIList);
            }
            else
            {
                if (optionUI.gameObject.activeSelf)
                {
                    optionUI.gameObject.SetActive(false);
                }
                else
                {
                    optionUI.gameObject.SetActive(true);
                }
            }
        }
        if(activeFullScreenUILList.Count == 0) // 닫기는 esc로 관리 List에 중복된 패널이 존재하기 때문
        {
            ToggleKeyDownAction(inventoryKey, inventoryList);
            ToggleKeyDownAction(equipmentKey, equipmentList);
            ToggleKeyDownAction(charInfoKey, statusList);
            ToggleKeyDownAction(mapKey, mapList);
        }


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
        statusList = new List<FullScreenUI>() { abillityStatusUI,playerImageUI, basicStatusUI, mainStatusUI };
        inventoryList = new List<FullScreenUI>() { abillityStatusUI,basicStatusUI, mainStatusUI, itemInformationUI, inventoryUI };
        equipmentList = new List<FullScreenUI>() { abillityStatusUI,basicStatusUI, mainStatusUI, itemInformationUI, equipmentUI };
        shopList = new List<FullScreenUI>() { abillityStatusUI,basicStatusUI, mainStatusUI, itemInformationUI, shopUI };
        levelUpList = new List<FullScreenUI>() { abillityStatusUI,basicStatusUI, levelUpUI };
        mapList = new List<FullScreenUI>() { travelUI, mapUI };


        foreach (FullScreenUI fscreen in allFullScreenUIList) // 모든 팝업에 이벤트 등록
        {
            fscreen.OnFocus += () => //헤더 포커스 이벤트
            {
                activeFullScreenUILList.Remove(fscreen);
                activeFullScreenUILList.AddFirst(fscreen);
                RefreshAllPopupDepth();
            };
        }

    }

    private void InitCloseAll() // 시작시 모든 팝업 닫기
    {
        CloseUIList(allFullScreenUIList);
        /*foreach (FullScreenUI fScreen in allFullScreenUIList)
        {
            CloseUI(fScreen);
        }*/
        //fullScreenBase.SetActive(false);
    }

    // 단축키 입력에 따라 팝업 열거나 닫기
    private void ToggleKeyDownAction(in KeyCode key, List<FullScreenUI> fScreens)
    {
        

        if (Input.GetKeyDown(key))
        {
            ToggleOpenCloseUIList(fScreens);

            if(fScreens != null) SetBase(key);

        }

        if(activeFullScreenUILList.Count > 0)
        {
            fullScreenBase.gameObject.SetActive(true);
        } else
        {
            fullScreenBase.gameObject.SetActive(false);
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
        activeFullScreenUILList.AddFirst(fScreen);
        fScreen.gameObject.SetActive(true);
        RefreshAllPopupDepth();
    }
    private void OpenUIList(List<FullScreenUI> fScreens)
    {
        if (fScreens == null) return;

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
        activeFullScreenUILList.Remove(fScreen);
        fScreen.gameObject.SetActive(false);
        RefreshAllPopupDepth();
    }
    private void CloseUIList(List<FullScreenUI> fScreens)
    {
        if (fScreens == null) return;

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
    void SetBase(in KeyCode key)
    {
        switch(key)
        {
            case KeyCode.C:
                SetBaseInform("스테이터스", menuIconList[0]);
                break;
            case KeyCode.I:
                SetBaseInform("인벤토리", menuIconList[1]);
                break;
            case KeyCode.E:
                SetBaseInform("장비", menuIconList[2]);
                break;
            default:
                break;
        }

    }
    void SetBaseInform(string name, Sprite icon)
    {
        menuName.text = name;
        menuIcon.sprite = icon;
    }
}