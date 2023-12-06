using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerStatusHandler playerStatusHandler;
    private PlayerStat max;
    private Animator anim;
    public LastPlayerController player;
    private SoundManager soundManager;
    private float comboResetTime = 1.5f;
    private float lastClickTime;
    [SerializeField] public int attackStaminaCost = 5; // 민열님과 얘기
    double nextAttackTime = 0f;

    public bool isParrying = false;
    public bool isGuarding = false;
    private float parryWindowEndTime = 0f;
    public bool canAttack = true;

    private int comboAttackClickCount = 0;
    private int manaRegainClickCount = 0;
    [SerializeField] public bool monsterToPlayerDamage;
    public Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;
    public bool comboAttack;
    private Test test;
    private int comboCount=1;

    // Start is called before the first frame update

    void Start()
    {
        canAttack = true;
        max = playerStatusHandler.GetStat();
        soundManager = SoundManager.instance;
    }
    private void Awake()
    {

        anim = GetComponent<Animator>();
        playerStatusHandler = GetComponent<PlayerStatusHandler>();
        test = GetComponent<Test>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDeffense();
        CheckAttackTime();
    }


    public void CheckDeffense()
    {
        if (isGuarding)
        {
            monsterToPlayerDamage = true;// 몬스터가 플레이어한테 데미지를 줌 
        }

        // 패링 가능한 상태에서만 패링이 가능하도록 체크
        if (Input.GetMouseButtonDown(1) && !isParrying)
        {
            isParrying = true;
            transform.Find("Parrying").gameObject.SetActive(true);
            parryWindowEndTime = Time.time + playerStatusHandler.currentParryTime;

            Debug.Log("Parry Start"+ playerStatusHandler.currentParryTime);
        }
        else if (Input.GetMouseButtonUp(1) && isParrying)
        {
            isParrying = false;
            transform.Find("Parrying").gameObject.SetActive(false);
            Debug.Log("Parry Success");
        }

        // 가드가 활성화되지 않은 상태에서 가드 가능한지 체크
        if (Input.GetMouseButtonDown(1) && !isGuarding)
        {
            canAttack = false;
            isGuarding = true;
            Debug.Log("Guard Start");
            transform.Find("Shield").gameObject.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1) && isGuarding)
        {
            canAttack = true;
            isGuarding = false;
            Debug.Log("Guard End");
            transform.Find("Shield").gameObject.SetActive(false);
        }

        // 패링 윈도우 종료 체크
        if (Time.time > parryWindowEndTime)
        {
            isParrying = false;
            transform.Find("Parrying").gameObject.SetActive(false);
            //Debug.Log("Parry Failed");
        }
    }


    private void ManaPlus()
    {
        Debug.Log("comboCount:" + comboCount);
        if (comboCount %3 ==0)
        {
            if (max.mana < playerStatusHandler.curretMana)//(현재마나  < 맥스마나 )
            {
                max.mana += 1;
            }
        }
    }

    private void ClickCount()
    {
        comboAttackClickCount += 1;
        lastClickTime = Time.time;
    }

    private void CheckAttackTime()
    {

        if (Time.time >= nextAttackTime)//다음 공격 가능 시간 
        {
            if (canAttack == true)
            {
                if (Input.GetMouseButtonDown(0) && player.isGrounded) //&& PopupUIManager.instance.activePopupLList.Count <= 0)
                {
                    //double sp = gameManager.playerStats.AttackSpeed + 1f; // AttackSpeed= 1 // 아이템 공속 감소?
                    nextAttackTime = Time.time + 1f; // / sp  <= 삭제함( 수정 필요 )
                    if (playerStatusHandler.currentStemina >= attackStaminaCost)
                    {
                        playerStatusHandler.currentStemina -= attackStaminaCost;
                        anim.SetTrigger("attack");
                        soundManager.PlayClip(test.attackSound);
                        ApplyDamage();
                    }
                }
            }
        }
    }

    private void RegainHp(int damage)
    {
        int heal = damage;
        if (playerStatusHandler.currentHp < playerStatusHandler.curretRegainHp)
        {
            playerStatusHandler.currentHp += heal / 4;
        }
    }

    private int DamageCalculator()
    {
        int modifiedAttackDamage = playerStatusHandler.currentDamage;
        if (comboAttackClickCount != 3)
        {
//            soundManager.PlayClip(test.attackSound);
            comboAttack = false;
        }
        else
        {
            anim.SetTrigger("combo");
            soundManager.PlayClip(test.comboAttackSound);
            playerStatusHandler.currentStemina -= attackStaminaCost * 2;
            modifiedAttackDamage *= 2;
            comboAttackClickCount = 0;
            comboAttack = true;
            comboCount += 1;
            ManaPlus();
        }
        modifiedAttackDamage = playerStatusHandler.CriticalCheck(modifiedAttackDamage);
        return modifiedAttackDamage;
    }

    private void ApplyDamage() // Add damage To Monster
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, playerStatusHandler.currentAttackRange, enemyLayer);
        //Debug.Log("enemyLayer : " + enemyLayer);
        //Debug.Log("hitEnemy : " + hitEnemies.Length);
        if (hitEnemies.Length != 0)
        {
            ClickCount();
            int damage = DamageCalculator();
            foreach (Collider2D enemyCollider in hitEnemies)
            {
                EnemyStatusHandler enemyhandler = enemyCollider.GetComponent<EnemyStatusHandler>();
                enemyhandler.TakeDamage(damage);
                RegainHp(damage);
            }
        }
    }


    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(attackPoint.position, stat.attackRange);

    //}
}