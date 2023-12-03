using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerStatusHandler playerStatusHandler;
    private PlayerStat stat;
    private Animator anim;
    public LastPlayerController player;

    private float comboResetTime = 1.5f;
    private float lastClickTime;
    [SerializeField] public int attackStaminaCost = 5; // 민열님과 얘기
    double nextAttackTime = 0f; // 어디서 쓰는지?

    public bool isParrying = false;
    public bool isGuarding = false;
    private float parryWindowEndTime = 0f; // 어디서 쓰는지?
    private bool canAttack = true;

    private int comboAttackClickCount = 0;
    private int manaRegainClickCount = 1;
    [SerializeField] public bool monsterToPlayerDamage;
    //public int damage;

    public Transform attackPoint;
    //public float attackRange = 1f;
    [SerializeField] private LayerMask enemyLayer;
    public bool comboAttack;

    // Start is called before the first frame update

    void Start()
    {
        canAttack = true;
        stat = playerStatusHandler.GetStat();
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerStatusHandler = GetComponent<PlayerStatusHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDeffense();
        CheckAttackTime();
        ResetClickCount();
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
            //parryWindowEndTime = Time.time + characterStats.ParryTime;
            parryWindowEndTime = Time.time + stat.parryTime;
            Debug.Log("Parry Start");
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


    private void ResetClickCount()
    {
        if (manaRegainClickCount == 10)
        {
            //if (characterStats.characterMana<characterStats.MaxMana)
            //{
            //    characterStats.characterMana += 1;
            //}
            // if (stat.mana < 4)
            // {
            //     stat.mana += 1;
            // }
            // manaRegainClickCount = 1;
        }
    }

    private void ClickCount()
    {
        comboAttackClickCount += 1;
        lastClickTime = Time.time;
        manaRegainClickCount += 1;
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
                    if (stat.stemina >= attackStaminaCost)
                    {
                        anim.SetTrigger("attack");

                        ApplyDamage();
                    }
                }
            }
        }
    }

    private void RegainAttack(int damage)
    {
        int heal = damage;
        if (stat.hp < stat.regainHp)
        {
            stat.hp += heal / 4;
        }
    }

    private int DamageCalculator()
    {
        int modifiedAttackDamage = stat.damage;
        if (comboAttackClickCount != 3)
        {

            stat.stemina -= attackStaminaCost;
            comboAttack = false;
        }
        else
        {
            anim.SetTrigger("combo");
            stat.stemina -= attackStaminaCost * 2;
            modifiedAttackDamage *= 2;
            comboAttackClickCount = 0;
            comboAttack = true;
        }
        modifiedAttackDamage = playerStatusHandler.CriticalCheck(modifiedAttackDamage);
        return modifiedAttackDamage;
    }

    private void ApplyDamage() // Add damage To Monster
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, stat.attackRange, enemyLayer);

        Debug.Log("enemyLayer : " + enemyLayer);
        Debug.Log("hitEnemy : " + hitEnemies.Length);
        if (hitEnemies.Length != 0)
        {

            ClickCount();
            int damage = DamageCalculator();
            foreach (Collider2D enemyCollider in hitEnemies)
            {
                EnemyStatusHandler enemyhandler = enemyCollider.GetComponent<EnemyStatusHandler>();
                enemyhandler.TakeDamage(damage);
                RegainAttack(damage);
                PlayerEvents.playerDamaged.Invoke(gameObject, damage);
            }
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(attackPoint.position, stat.attackRange);
    // }
}