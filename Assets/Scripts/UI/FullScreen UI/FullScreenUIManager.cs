using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenUIManager : MonoBehaviour
{
    public static FullScreenUIManager instance;

    [Header("UI Base")]
    [SerializeField] private Image menuIcon;
    [SerializeField] private List<Sprite> menuIconList;
    [SerializeField] private TMP_Text menuName;
    [SerializeField] private TMP_Text soulCount;
    [SerializeField] private TMP_Text weightTxt;
    [SerializeField] private GameObject fullScreenBase;
    [SerializeField] private Inventory inven;
    [SerializeField] private PlayerStatusHandler playerStatusHandler;
    [SerializeField] private PlayerStat playerMaxStat;
    [SerializeField] private UISwitch uiSwitch;
    [SerializeField] private FItemInformationUI fInform;

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
    public FullScreenUI mainStatusUI;
    public FullScreenUI basicStatusUI;
    public FullScreenUI abillityStatusUI;
    public FullScreenUI playerImageUI;
    public FullScreenUI inventoryUI;
    public FullScreenUI itemInformationUI;
    public FullScreenUI equipmentUI;
    public FullScreenUI shopUI;
    public FullScreenUI levelUpUI;
    public FullScreenUI optionUI;
    public FullScreenUI mapUI; //
    public FullScreenUI travelUI; //
    public GameObject switchPanelUI; 
    public GameObject itemSelectUI; 

    [Header("UI 그룹")] // 여러개의 UI를 넣어두고 0, 1, 2를 처음에 세팅 ... 표시전환 누르면 n번째가 현재 들어와있는것 중 다음거를 부름?
    [SerializeField] private List<FullScreenUI> statusList;
    [SerializeField] private List<FullScreenUI> inventoryList;
    [SerializeField] private List<FullScreenUI> equipmentList;
    [SerializeField] private List<FullScreenUI> shopList;
    [SerializeField] private List<FullScreenUI> levelUpList;
    [SerializeField] private List<FullScreenUI> mapList;

    [SerializeField] public LinkedList<FullScreenUI> activeFullScreenUILList;

    [SerializeField] public List<FullScreenUI> allFullScreenUIList;
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
        instance = this;
        activeFullScreenUILList = new LinkedList<FullScreenUI>();
        //menuIconList = new List<Sprite>(); Resources/,...
        

    }
    private void Start()
    {
        inven = Inventory.instance;
        playerStatusHandler = GameManager.instance.player.GetComponent<PlayerStatusHandler>();
        playerMaxStat = playerStatusHandler.GetStat();
        //uiSwitch = switchPanelUI.GetComponent<UISwitch>();
        uiSwitch = UISwitch.instance;
        //fInform = FItemInformationUI.instance;
        fInform = itemInformationUI.GetComponent<FItemInformationUI>();

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
                uiSwitch.ClearSwitch();
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

        if ((inven.currentNPC != null) && (inven.currentNPC.isInteractable)) //inventory.instance.currentNPC는 Player.currentNPC로 변경 예정
        {
            if (inven.currentNPC.npcName.Equals("Shop"))
            {
                ToggleKeyDownAction(npcKey, shopList);
            } else if(inven.currentNPC.npcName.Equals("Stat"))
            {
                ToggleKeyDownAction(npcKey, levelUpList);
            }
        }

        if(Input.GetKeyDown(KeyCode.Y))
        {
            if(activeFullScreenUILList.Count > 0)
            {
                uiSwitch.ChangeSwitch();

            }
        }

    }

    private void Init()
    {
        allFullScreenUIList = new List<FullScreenUI>() // 리스트 초기화
        {
            mainStatusUI, basicStatusUI, playerImageUI, abillityStatusUI, inventoryUI, itemInformationUI, equipmentUI, shopUI, levelUpUI
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
    }

    // 단축키 입력에 따라 팝업 열거나 닫기
    private void ToggleKeyDownAction(in KeyCode key, List<FullScreenUI> fScreens)
    {
        
        if (Input.GetKeyDown(key))
        {
            ToggleOpenCloseUIList(fScreens);

            if (fScreens != null) {
                SetBase(key);
                SetPosition(fScreens);
            } 

        }
        if (activeFullScreenUILList.Count > 0)
        {
            fullScreenBase.gameObject.SetActive(true);
            weightTxt.text = $"{playerStatusHandler.currentWeight} / {playerMaxStat.weight}" +
            $"({(playerStatusHandler.currentWeight / playerMaxStat.weight * 100):F0})%";
            soulCount.text = $"{inven.SoulCount:N0}";
/*                weightValue.text = $"{playerStatusHandler.currentWeight} / {playerMaxStat.weight}" +
            $"({(playerStatusHandler.currentWeight / playerMaxStat.weight * 100):F0}%)";*/
        }
        else
        {
            fullScreenBase.gameObject.SetActive(false);
        }
        uiSwitch.Init();
        fInform.ClearInform();


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
            case KeyCode.F:
                if(inven.currentNPC.npcName == "Shop")
                    SetBaseInform("상점", menuIconList[3]);

                if(inven.currentNPC.npcName == "Stat")
                    SetBaseInform("레벨업", menuIconList[4]);
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

    void SetPosition(List<FullScreenUI> fScreens)
    {
        uiSwitch.ClearSwitch();
        int length = fScreens.Count-1;
        for(int i = fScreens.Count-1; i >= 0; i--)
        {
            if(i == length)
            {
                fScreens[length].GetComponent<RectTransform>().SetPositionAndRotation(leftPanelPosition, Quaternion.identity);
            } else if(i == length-1) {
                fScreens[length - 1].GetComponent<RectTransform>().SetPositionAndRotation(centerPanelPosition, Quaternion.identity);
            } else
            {
                fScreens[i].GetComponent<RectTransform>().SetPositionAndRotation(rightPanelPosition, Quaternion.identity);
            }


            if (fScreens.Count >= 4)
            {
                uiSwitch.AddSwitch(fScreens.Count - 2);
                //패널 생성
            }
            //마지막이 left
            //마지막-1이 센터
            // 나머지가 라이트
            
        }
        // 위치를 정해주고
        // switch를 3번째 위치 패널 개수마다 생성해주고(2개 이상일때) or 모든 개수를 셋을때 4이상일때부터 생성
        // switch의 색깔을 정해주는건 Switch 스크립트에서 하자 FF4242
    }
}