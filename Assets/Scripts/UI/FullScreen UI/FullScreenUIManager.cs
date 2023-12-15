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

    public List<FullScreenUI> currentUI; // ���� ���� UIList Ȯ�� => �����ܰ� �޴� �̸� �������ֱ� ����?
    public int uiIndex;

    [Space]
    public KeyCode escapeKey = KeyCode.Escape;
    public KeyCode inventoryKey = KeyCode.I;
    public KeyCode equipmentKey = KeyCode.E;
    public KeyCode charInfoKey = KeyCode.C;
    public KeyCode npcKey = KeyCode.F;
    public KeyCode mapKey = KeyCode.M;

    [Header("UI���")]
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

    [Header("UI �׷�")] // �������� UI�� �־�ΰ� 0, 1, 2�� ó���� ���� ... ǥ����ȯ ������ n��°�� ���� �����ִ°� �� �����Ÿ� �θ�?
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
                itemSelectUI.SetActive(false);
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
        if(activeFullScreenUILList.Count == 0) // �ݱ�� esc�� ���� List�� �ߺ��� �г��� �����ϱ� ����
        {
            ToggleKeyDownAction(inventoryKey, inventoryList);
            ToggleKeyDownAction(equipmentKey, equipmentList);
            ToggleKeyDownAction(charInfoKey, statusList);
            ToggleKeyDownAction(mapKey, mapList);
        }

        if ((inven.currentNPC != null) && (inven.currentNPC.isInteractable)) //inventory.instance.currentNPC�� Player.currentNPC�� ���� ����
        {
            if (inven.currentNPC.npcName.Equals("Shop"))
            {
                ToggleKeyDownAction(npcKey, shopList);
            }
            else if (inven.currentNPC.npcName.Equals("Stat"))
            {
                ToggleKeyDownAction(npcKey, levelUpList);
            }
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            if(activeFullScreenUILList.Count > 0)
            {
                uiSwitch.ChangeSwitch();

            }
        }

    }

    private void Init()
    {
        allFullScreenUIList = new List<FullScreenUI>() // ����Ʈ �ʱ�ȭ
        {
            mainStatusUI, basicStatusUI, playerImageUI, abillityStatusUI, inventoryUI, itemInformationUI, equipmentUI, shopUI, levelUpUI, travelUI, mapUI
        };
        statusList = new List<FullScreenUI>() { abillityStatusUI,playerImageUI, basicStatusUI, mainStatusUI };
        inventoryList = new List<FullScreenUI>() { abillityStatusUI,basicStatusUI, mainStatusUI, itemInformationUI, inventoryUI };
        equipmentList = new List<FullScreenUI>() { abillityStatusUI,basicStatusUI, mainStatusUI, itemInformationUI, equipmentUI };
        shopList = new List<FullScreenUI>() { abillityStatusUI,basicStatusUI, mainStatusUI, itemInformationUI, shopUI };
        levelUpList = new List<FullScreenUI>() { abillityStatusUI,basicStatusUI, levelUpUI };
        mapList = new List<FullScreenUI>() { travelUI, mapUI };


        foreach (FullScreenUI fscreen in allFullScreenUIList) // ��� �˾��� �̺�Ʈ ���
        {
            fscreen.OnFocus += () => //��� ��Ŀ�� �̺�Ʈ
            {
                activeFullScreenUILList.Remove(fscreen);
                activeFullScreenUILList.AddFirst(fscreen);
                
                RefreshAllPopupDepth();
            };
        }

    }

    public void InitCloseAll() // ���۽� ��� �˾� �ݱ�
    {
        CloseUIList(allFullScreenUIList);
    }

    // ����Ű �Է¿� ���� �˾� ���ų� �ݱ�
    private void ToggleKeyDownAction(in KeyCode key, List<FullScreenUI> fScreens)
    {
        
        if (Input.GetKeyDown(key))
        {
            ToggleOpenCloseUIList(fScreens);

            if (fScreens != null) {
                SetBase(key);
                SetPosition(fScreens);
            }
            fInform.ClearInform();
        }
        if (activeFullScreenUILList.Count > 0)
        {
            fullScreenBase.gameObject.SetActive(true);
            weightTxt.text = $"{playerStatusHandler.currentWeight} / {playerMaxStat.weight}" +
            $"({((playerStatusHandler.currentWeight * 100f) / playerMaxStat.weight):F0}%)";
            soulCount.text = $"{inven.SoulCount:N0}";

            
            /*                weightValue.text = $"{playerStatusHandler.currentWeight} / {playerMaxStat.weight}" +
                        $"({(playerStatusHandler.currentWeight / playerMaxStat.weight * 100):F0}%)";*/
        }
        else
        {
            fullScreenBase.gameObject.SetActive(false);
        }
        uiSwitch.Init();
    }

    // �˾��� ���¿� ���� ���ų� �ݱ�(opened / closed)
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

    // �˾��� ���� ��ũ�帮��Ʈ�� ��ܿ� �߰�
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

    // �˾��� �ݰ� ��ũ�帮��Ʈ���� ����
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

    //��ũ�帮��Ʈ �� ��� �˾��� �ڽ� ���� ���ġ
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
                SetBaseInform("�������ͽ�", menuIconList[0]);
                break;
            case KeyCode.I:
                SetBaseInform("�κ��丮", menuIconList[1]);
                break;
            case KeyCode.E:
                SetBaseInform("���", menuIconList[2]);
                break;
            case KeyCode.F:
                if(inven.currentNPC.npcName == "Shop")
                    SetBaseInform("����", menuIconList[3]);

                if(inven.currentNPC.npcName == "Stat")
                    SetBaseInform("������", menuIconList[4]);
                break;
            case KeyCode.M:
                SetBaseInform("����", menuIconList[5]);
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

        if(fScreens.Count < 3)
        {
            fScreens[1].GetComponent<RectTransform>().SetPositionAndRotation(leftPanelPosition + new Vector2(300, 0), Quaternion.identity);
            fScreens[0].GetComponent<RectTransform>().SetPositionAndRotation(rightPanelPosition, Quaternion.identity);
        } else
        {
            for (int i = fScreens.Count - 1; i >= 0; i--)
            {
                if (i == length)
                {
                    fScreens[length].GetComponent<RectTransform>().SetPositionAndRotation(leftPanelPosition, Quaternion.identity);
                }
                else if (i == length - 1)
                {
                    fScreens[length - 1].GetComponent<RectTransform>().SetPositionAndRotation(centerPanelPosition, Quaternion.identity);
                }
                else
                {
                    fScreens[i].GetComponent<RectTransform>().SetPositionAndRotation(rightPanelPosition, Quaternion.identity);
                }


                if (fScreens.Count >= 4)
                {
                    uiSwitch.AddSwitch(fScreens.Count - 2);
                    //�г� ����
                }
                //�������� left
                //������-1�� ����
                // �������� ����Ʈ

            }
        }


        // ��ġ�� �����ְ�
        // switch�� 3��° ��ġ �г� �������� �������ְ�(2�� �̻��϶�) or ��� ������ ������ 4�̻��϶����� ����
        // switch�� ������ �����ִ°� Switch ��ũ��Ʈ���� ���� FF4242
    }
}