using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveMenuManager : MonoBehaviour
{
    public static SaveMenuManager instance;

    public List<GameObject> menus;
    private Stack<GameObject> activingMenus;
    private GameObject activeMenu;

    private FastTravel fastTravel;

    private void Awake()
    {
        instance = this;
        activingMenus = new Stack<GameObject>();
        fastTravel = GetComponent<FastTravel>();
        AllDeActiveMenu();
    }
    
    public void ActiveMenu(GameObject menu)
    {
        if (activingMenus.Contains(menu))
            return;
        menu.SetActive(true);
        activingMenus.Push(menu);
        activeMenu = menu;
    }

    public void DeActiveMenu()
    {
        activingMenus.Pop().SetActive(false);
        if (activingMenus.Count > 0)
            activeMenu = activingMenus.Peek();
    }
    
    public void AllDeActiveMenu()
    {
        for (int i = 0; i < menus.Count; i++) 
        {
            menus[i].SetActive(false);
        }
        activingMenus.Clear();
    }

    public void AddTravel(Travel travel)
    {
        fastTravel.InputTravel(travel);
    }
}
