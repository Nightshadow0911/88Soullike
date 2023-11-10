using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    private void Awake()
    {
        instance = this;
    }
    public List<ItemSO> itemDB = new List<ItemSO>();

    public GameObject fieldItemPrefab;
    public Vector3[] pos;

    private void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject go = Instantiate(fieldItemPrefab, pos[i], Quaternion.identity);
            go.GetComponent<FieldItems>().SetItem(itemDB[Random.Range(0, itemDB.Count)]);
        }
    }
    // 박스와 몬스터에 아이템 정보를 세팅하는 함수
    // 박스나 몬스터 파괴, 처치시에 세팅된 아이템을 떨구는 함수, 적당한 애니메이션 필요
    // 박스.cs  -> 데미지가 얼마 들어오면 or 몇대 맞으면 사라짐, 사라지면서 디졸브 애니메이션, 열린상자로 이미지 변경
    // 열린상자에 플레이어가 닿으면 아이템 획득(바로 획득할지?, 어떤걸 획득했다고 텍스트로 알려줄지?, 드랍템을 리스트로 띄워서 선택한 아이템을 획득하게 할지?)
}
