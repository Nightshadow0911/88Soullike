using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatScript : StatFunctioParent
{
    public StatScriptableObject enemyStatObject;

    private int currentHealth;
    
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
