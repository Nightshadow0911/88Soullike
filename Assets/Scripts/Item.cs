using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    ItemSO curItem; // 이름, 이미지, 타입, 파워, 설명

    public bool Use()
    {
        return false; // 아이템 사용 성공 여부
    }
}
