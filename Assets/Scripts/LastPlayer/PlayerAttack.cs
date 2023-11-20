using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private GameManager gameManager;
    public LastPlayerController player;
    public CharacterStats characterStats;

    private float lastAttackTime = 0f;
    public float attackRate = 1f;
    float nextAttackTime = 0f;
    private int attackClickCount = 1;
    public bool canTakeDamage = true;
    public int damage = 10;

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
        if (Time.time > lastAttackTime + 5f)
        {
            attackClickCount = 1;
        }
        CheckAttackTime();
        //Attack();
    }

    private void CheckAttackTime()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0) && player.isGrounded && PopupUIManager.instance.activePopupLList.Count <= 0)
            {
                nextAttackTime = Time.time + 0.5f / attackRate;
                Attack();
            }
        }
    }

    public void Attack()
    {
        if (gameManager.playerStats.characterStamina >= player.attackStaminaCost)
        {
            gameManager.playerStats.characterStamina -= player.attackStaminaCost;
            anim.SetTrigger("attack");
            gameManager.playerStats.AttackDamage(damage);
            int modifiedAttackDamage = damage;
            if (attackClickCount != 0 && attackClickCount % 3 == 0)
            {
                gameManager.playerStats.characterStamina -= player.comboStaminaCost;
                anim.SetTrigger("combo");
                modifiedAttackDamage += 10;
                attackClickCount = 0;
            }
            ApplyDamage(modifiedAttackDamage);
        }
    }

    //Debug.Log(canTakeDamage);//PlayerToMonster

    private void ApplyDamage(int damage) // Add damage To Monster
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Boss_DB"))
            {
                lastAttackTime = Time.time;
                attackClickCount++;
                DeathBringerEnemy deathBringer = enemyCollider.GetComponent<DeathBringerEnemy>();
                if (deathBringer != null)
                {
                    deathBringer.TakeDamage(damage);
                    PlayerEvents.playerDamaged.Invoke(gameObject, damage);
                }
            }
            else if (enemyCollider.CompareTag("Boss_Archer"))
            {
                Boss_Archer boss_archer = enemyCollider.GetComponent<Boss_Archer>();
                if (boss_archer != null)
                {
                    //Debug.Log("Deal" + characterStats.characterNomallAttackDamage + " damage to Boss Archer.");
                    //boss_archer.TakeDamage(attackDamage);
                    boss_archer.TakeDamage(characterStats.characterNomallAttackDamage);
                }
            }
            else if (enemyCollider.CompareTag("skeleton"))
            {
                skeletonEnemy skeleton = enemyCollider.GetComponent<skeletonEnemy>();
                if (skeleton != null)
                {
                    //Debug.Log("Deal" + characterStats.characterNomallAttackDamage + " damage to Skeleton.");
                    skeleton.TakeDamage(characterStats.characterNomallAttackDamage);
                }
            }
            else if (enemyCollider.CompareTag("archer"))
            {
                archerEnemy archer = enemyCollider.GetComponent<archerEnemy>();
                if (archer != null)
                {
                    //Debug.Log("Deal " + characterStats.characterNomallAttackDamage + " damage to Archer.");
                    archer.TakeDamage(characterStats.characterNomallAttackDamage);
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