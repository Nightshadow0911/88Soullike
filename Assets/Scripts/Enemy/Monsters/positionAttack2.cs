using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class positionAttack2 : PositionAttackController
{
    [SerializeField] private PositionAttackData spellSpawnData;
    private PositionAttack positionAttack;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void DestroyProjectile()
    {
        Vector3 position = Vector3.down;
        positionAttack.CreateProjectile(position, spellSpawnData);
    }
}

