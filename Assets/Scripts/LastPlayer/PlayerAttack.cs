using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private GameManager gameManager;
    public LastPlayerController player;
    public CharacterStats characterStats;

    private float clickCountResetTime = 1.5f; // 클릭 카운터를 초기화하는데 걸리는 시간
    private float lastClickTime;

    double nextAttackTime = 0f;
    public float parryWindowDuration = 0.5f; // 패링이 가능한 시간 간격
    public bool isParrying = false;
    public bool isGuarding = false;
    private float parryWindowEndTime = 0f;

    [SerializeField] private int attackClickCount = 1;
    [SerializeField] public bool monsterToPlayerDamage;
    //public int damage;

    public Transform attackPoint;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask enemyLayer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = GameManager.Instance;

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
            parryWindowEndTime = Time.time + parryWindowDuration;
            Debug.Log("Parry Start");
        }
        else if (Input.GetMouseButtonUp(1) && isParrying)
        {
            isParrying = false;
            Debug.Log("Parry Success");
        }

        // 가드가 활성화되지 않은 상태에서 가드 가능한지 체크
        if (Input.GetMouseButtonDown(1) && !isGuarding)
        {
            isGuarding = true;
            Debug.Log("Guard Start");
        }
        else if (Input.GetMouseButtonUp(1) && isGuarding)
        {
            isGuarding = false;
            Debug.Log("Guard End");
        }

        // 패링 윈도우 종료 체크
        if (Time.time > parryWindowEndTime)
        {
            isParrying = false;
            //Debug.Log("Parry Failed");
        }
    }


    private void ResetClickCount()
    {
        // 클릭 카운터 초기화
        if (Time.time - lastClickTime > clickCountResetTime)
        {
            attackClickCount = -1;
        }
    }

    private void ClickCount()
    {
        attackClickCount += 1;
        lastClickTime = Time.time;
    }

    private void CheckAttackTime()
    {
        if (Time.time >= nextAttackTime)//다음 공격 가능 시간 
        {
            if (Input.GetMouseButtonDown(0) && player.isGrounded && PopupUIManager.instance.activePopupLList.Count <= 0)
            {
                double sp = gameManager.playerStats.AttackSpeed + 1f;
                nextAttackTime = Time.time + 1f / +sp;


                if (player.isSitting == false)
                {
                    Attack();
                }
                else
                {
                    CrouchAttack();
                }
            }
        }
    }

    public void Attack()
    {
        if (gameManager.playerStats.characterStamina >= player.attackStaminaCost)
        {
            gameManager.playerStats.characterStamina -= player.attackStaminaCost;
            anim.SetTrigger("attack");
            gameManager.playerStats.AttackDamage();
            int modifiedAttackDamage = gameManager.playerStats.NormalAttackDamage;
            if (attackClickCount != 0 && attackClickCount % 2==0)
            {
                gameManager.playerStats.characterStamina -= player.comboStaminaCost;
                anim.SetTrigger("combo");
                modifiedAttackDamage *= 2;
                ApplyDamage(modifiedAttackDamage);
                attackClickCount = -1;
            }
            ApplyDamage(modifiedAttackDamage);

        }
    }
    public void CrouchAttack()
    {
        if (gameManager.playerStats.characterStamina >= player.attackStaminaCost)
        {
            gameManager.playerStats.characterStamina -= player.attackStaminaCost;
            anim.SetTrigger("crouchAttack");
            gameManager.playerStats.AttackDamage();
            int a = gameManager.playerStats.NormalAttackDamage;
            int modifiedAttackDamage = a / 2;
            ApplyDamage(modifiedAttackDamage);

        }
    }

    private void RegainAttack()
    {
        int heal = gameManager.playerStats.totalDamage;
        if (gameManager.playerStats.characterHp < gameManager.playerStats.characterRegainHp)
        {
            gameManager.playerStats.characterHp += heal / 4;
        }
    }

    private void ApplyDamage(int damage) // Add damage To Monster
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Boss_DB"))
            {
                ClickCount();
                DeathBringerEnemy deathBringer = enemyCollider.GetComponent<DeathBringerEnemy>();
                if (deathBringer != null)
                {
                    deathBringer.TakeDamage(gameManager.playerStats.totalDamage);
                    RegainAttack();
                    PlayerEvents.playerDamaged.Invoke(gameObject, damage);
                }
            }
            else if (enemyCollider.CompareTag("Boss_Archer"))
            {
                ClickCount();
                Boss_Archer boss_archer = enemyCollider.GetComponent<Boss_Archer>();
                if (boss_archer != null)
                {
                    boss_archer.TakeDamage(gameManager.playerStats.totalDamage);
                    RegainAttack();
                    PlayerEvents.playerDamaged.Invoke(gameObject, damage);
                }
            }
            else if (enemyCollider.CompareTag("skeleton"))
            {
                ClickCount();
                skeletonEnemy skeleton = enemyCollider.GetComponent<skeletonEnemy>();
                if (skeleton != null)
                {
                    skeleton.TakeDamage(gameManager.playerStats.totalDamage);
                    RegainAttack();
                    PlayerEvents.playerDamaged.Invoke(gameObject, damage);
                }
            }
            else if (enemyCollider.CompareTag("archer"))
            {
                ClickCount();
                archerEnemy archer = enemyCollider.GetComponent<archerEnemy>();
                if (archer != null)
                {
                    archer.TakeDamage(gameManager.playerStats.totalDamage);
                    RegainAttack();
                    PlayerEvents.playerDamaged.Invoke(gameObject, damage);

                }
            }
        }
    }



    private void OnDrawGizmos()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }


}