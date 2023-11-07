using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettableSlot : MonoBehaviour
{
    EquipmentUI equip;
    public int itemIndex;
    private void Start()
    {
        equip = EquipmentUI.instance;

    }
    public void SetQuickSlot()
    {
        equip.quickSlots[equip.setSlotIndex].SetQuickSlotItem(itemIndex);
        equip.settableListPanel.SetActive(false);

        ResetSettableSlot();
    }

    private void ResetSettableSlot()
    {
        foreach (Transform setTrans in equip.settableListHolder)
        {
            Destroy(setTrans.gameObject);
            //setTrans.gameObject.SetActive(false);

        }
    }
}
