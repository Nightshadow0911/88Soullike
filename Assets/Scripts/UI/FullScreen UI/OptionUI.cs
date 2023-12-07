using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Button statusBtn;
    public Button inventoryBtn;
    public Button equipmentBtn;
    public Button optionBtn;

    FullScreenUIManager fManager;

    void Start()
    {
        fManager = FullScreenUIManager.instance;
    }
    public void clickBtn(Button currentBtn)
    {
        
    }

}
