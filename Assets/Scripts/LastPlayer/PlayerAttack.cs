using System.Collections;
using System.Collections.Generic;
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
        CheckInput();
        if (Time.time > lastAttackTime + 5f)
        {
            attackClickCount = 1;
        }
        CheckAttackTime();
        //CrouchAttack();
    }
    public void CheckInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            canTakeDamage = false;
            Debug.Log("누름 ");
        }
        else if (Input.GetMouseButtonUp(1))
        {
            canTakeDamage = true;
            Debug.Log("땜");
        }
    }
    private void CheckAttackTime()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0) && player.isGrounded && PopupUIManager.instance.activePopupLList.Count <= 0&& player.isSitting==false)
            {
                nextAttackTime = Time.time + 0.5f / attackRate;
                Attack();
            }
            if (Input.GetMouseButtonDown(0) && player.isGrounded && PopupUIManager.instance.activePopupLList.Count <= 0&& player.isSitting)
            {
                nextAttackTime = Time.time + 0.5f / attackRate;
                CrouchAttack();
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
            if (attackClickCount != 0 && attackClickCount % 3 == 0)
            {
                gameManager.playerStats.characterStamina -= player.comboStaminaCost;
                anim.SetTrigger("combo");
                modifiedAttackDamage *=2;
                Debug.Log("combo");
                attackClickCount = 0;
                ApplyDamage(modifiedAttackDamage);
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
            int modifiedAttackDamage = a/2;
            ApplyDamage(modifiedAttackDamage);
        }
    }



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
                    deathBringer.TakeDamage(gameManager.playerStats.totalDamage);
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
