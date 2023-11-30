using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatScript : StatFunctioParent
{
    public StatScriptableObject playerStatObject;
    public int currentHealth;
    private int stemina;

    void OnTriggerEnter(Collider other) //예시 
    {
        if (other.CompareTag("Enemy"))
        {   
            EnemyStatScript enemyStatScript = other.GetComponent<EnemyStatScript>();
            bool isDead = TakeDamage(1, enemyStatScript.GetCurrentHealth());
        }
    }
    
}
