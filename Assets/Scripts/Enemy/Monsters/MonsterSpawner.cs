using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour 
{
    public GameObject[] monster; // ��ȯ�� �Ϲ� ���� ������ �迭
    public GameObject Playerprefab;

    // Start is called before the first frame update
    void Start() //�ʿ� ��ȯ�� ���Ϳ� ��ǥ�� �Է��ؼ� �߰�
    {
        SpawnMonster(monster[0], new Vector3(1.0f, 1.0f, 1.0f));
        SpawnMonster(monster[1], new Vector3(8.0f, 3.0f, 1.0f));
        SpawnMonster(monster[2], new Vector3(15.0f, 3.0f, 1.0f));
        GameManager.instance.player = Instantiate(Playerprefab, new Vector2(-15f, 3f),Quaternion.identity);
    }

    public void SpawnMonster(GameObject curmonster, Vector3 spawnPosition)
    {
        GameObject curMonster = Instantiate(curmonster);
        curMonster.transform.position = spawnPosition;
    }
}