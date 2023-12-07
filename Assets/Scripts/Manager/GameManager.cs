using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    public GameObject[] monsters;

    // 캐릭터 스텟
    public CharacterStats playerStats;
    public LastPlayerController lastPlayerController;
    public PlayerAttack playerAttack;
    private void Awake()
    {
        instance = this;
        lastPlayerController = player.GetComponent<LastPlayerController>();
        playerAttack = player.GetComponent<PlayerAttack>();
    }

    public void CanAttack()
    {
        playerAttack.canAttack =  true;
    }
    public void CantAttack()
    {
        playerAttack.canAttack = false;
    }
}
