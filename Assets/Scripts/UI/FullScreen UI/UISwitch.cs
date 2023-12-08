using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISwitch : MonoBehaviour
{
    public static UISwitch instance;

    [SerializeField] private GameObject switchPrefab;
    [SerializeField] private Transform switchHolder;
    [SerializeField] private List<GameObject> switches;
    [SerializeField] private int activeSwitchCount;
    [SerializeField] private int switchIndex;
    [SerializeField] private Color32 color = new Color32(255, 66, 66, 255); //new Color(255, 66, 66, 1);
    [SerializeField] private Color whiteColor = Color.white;
    [SerializeField] private List<FullScreenUI> activeUI;

    [SerializeField] private FullScreenUIManager fManager;

    void Awake()
    {
        instance = this;

        switches = new List<GameObject>(5);


        for (int i = 0; i < switchHolder.childCount; i++)
        {
            GameObject child = switchHolder.GetChild(i).gameObject;
            switches.Add(child);
        }

    }
    private void Start()
    {
        fManager = FullScreenUIManager.instance;
        activeUI = new List<FullScreenUI>();
    }

    public void Init()
    {
        activeUI.Clear();
        foreach (FullScreenUI df in fManager.activeFullScreenUILList)
        {
            activeUI.Add(df);
        }

    }


    public void ChangeSwitch() //표시전환 키가 눌리면, active 리스트 수가 4 이상이면 가능
    {


        activeSwitchCount = 0;
        foreach (GameObject sw in switches)
        {
            if (sw.activeSelf)
            {
                activeSwitchCount++;
            }
        }
        if (switchIndex == activeSwitchCount - 1)
        {
            return;
        }
        Debug.Log("sw"+switchIndex);
        Debug.Log("ac"+activeSwitchCount);


        switchIndex++;

        if (switchIndex >= activeSwitchCount)
        {
            switchIndex = 0;
            
        }

        for (int i = 0; i < switches.Count; i++)
        {
            switches[i].GetComponent<Image>().color = whiteColor;
        }
        switches[switchIndex].GetComponent<Image>().color = color;

        SwitchPanel();
    }


    public void ClearSwitch()
    {
        foreach (GameObject sw in switches)
        {
            sw.SetActive(false);
        }
    }

    public void AddSwitch(int switchCount)
    {
        switchIndex = 0;
        for (int i = 0; i < switchCount; i++)
        {
            switches[i].SetActive(true);
        }
        for (int i = 0; i < switchCount; i++)
        {
            switches[i].GetComponent<Image>().color = whiteColor;
        }
        switches[0].GetComponent<Image>().color = color;
    }

    public void SwitchPanel()
    {


        for (int i = 0; i < activeUI.Count - 2; i++)
        {
            activeUI[activeUI.Count - switchIndex].SetFirst();
        }

    }


}
