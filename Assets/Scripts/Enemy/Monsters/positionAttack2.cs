using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class positionAttack2 : PositionAttackController
{
    [SerializeField] private PositionAttackData spellSpawnData;
    private PositionAttack positionAttack;
    // Start is called before the first frame update

    private void Awake()
    {
        positionAttack = GetComponent<PositionAttack>();
    }

    protected override void DestroyProjectile()
    {
        Vector3 position = transform.position;
        positionAttack.CreateProjectile(position, spellSpawnData);
        Invoke("DeActive", 1f);
    }

    private void DeActive()
    {
        gameObject.SetActive(false);
    }
}

