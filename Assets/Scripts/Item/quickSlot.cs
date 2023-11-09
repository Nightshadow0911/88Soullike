using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class quickSlot : MonoBehaviour
{
    public KeyCode slotKey;
    public Item slotItem;
    public Image itemIcon;
    public int slotIndex; // 퀵슬롯의 인덱스
    public int invenIndex; // 사용할 인벤토리 슬롯의 인덱스

    public void SetQuickSlotItem(int slotnum) // 인벤토리 아이템의 슬롯 넘버와 이미지를 가져온다. 슬롯 아이템에는 인벤토리의 슬롯 번호의 아이템을 지정
    {
        invenIndex = slotnum;
        slotItem = InventoryUI.instance.slots[slotnum].item;

        itemIcon.sprite = slotItem.sprite;
        itemIcon.transform.localScale = Vector3.one * 0.4f;
        itemIcon.gameObject.SetActive(true);

        Equipment.instance.quickSlotList[slotIndex] = slotItem;
    }

    public void UseQuickSlotItem() //키 입력되면 EquipmentUI.quickSlots[키 입력에 해당하는 숫자].UseQuickSlotItem();
    {
        InventoryUI.instance.slots[invenIndex].ApplyUse();
    }

    public void DisplaySettableItem() //인벤토리중 소모품을 모두 보여줌 / 스크롤뷰
    {
        EquipmentUI.instance.setSlotIndex = slotIndex;
        EquipmentUI.instance.settableListPanel.SetActive(true);
        EquipmentUI.instance.DrawquickSlot();
    }

    // 퀵슬롯 버튼을 누르면 슬롯을 초기화하고 아이템 할당창을 활성화
    // 아이템 할당창에는 내 인벤토리 중 소모품들만 보여줌
    // 할당창에서 아이템 슬롯을 누르면 처음 누른 퀵슬롯에 그 아이템을 할당
    // 퀵슬롯에 해당하는 숫자를 누르면(게임매니저 업데이트) 아이템 사용(InventoryUI.instance.Slot.ApplyUse())
}
