using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour 
{
    public GameObject[] monster; // 소환할 일반 몬스터 프리팹 배열
    public GameObject Playerprefab;

    // Start is called before the first frame update
    void Start() //맵에 소환할 몬스터와 좌표를 입력해서 추가
    {
        SpawnMonster(monster[0], new Vector3(1.0f, 1.0f, 1.0f));
        SpawnMonster(monster[1], new Vector3(8.0f, 3.0f, 1.0f));
        SpawnMonster(monster[2], new Vector3(15.0f, 3.0f, 1.0f));
        GameManager.Instance.player = Instantiate(Playerprefab, new Vector2(-15f, 3f),Quaternion.identity);
    }

    public void SpawnMonster(GameObject curmonster, Vector3 spawnPosition)
    {
        GameObject curMonster = Instantiate(curmonster);
        curMonster.transform.position = spawnPosition;
    }
}