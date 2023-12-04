using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public GameObject player;
    public GameObject[] monsters;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    // 캐릭터 스텟
    public CharacterStats playerStats;
    public LastPlayerController lastPlayerController;
    public PlayerAttack playerAttack;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
           // Destroy(gameObject);
        }
        lastPlayerController = player.GetComponent<LastPlayerController>();
        playerAttack = player.GetComponent<PlayerAttack>();
    }
}
