using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill : MonoBehaviour
{
    [SerializeField] private SkillSO curSkill;

    [SerializeField] private SkillType type;
    [SerializeField] private bool activeType;
    [SerializeField] private string skillName;
    [SerializeField] private Sprite skillIcon;
    [SerializeField] private List<SkillEffect> efts;
    [SerializeField] private int power; // ������ ����(ex: ��ų���ݷ� * power) or �����ð� ��
    [SerializeField] private int cost; // ���� �ڽ�Ʈ
    [SerializeField] private List<String> descriptiion;
    [SerializeField] private PropertyType skillProperty;
    [SerializeField] private int price; // ����(�������� �춧�� ������, �����ǸŰ� �Ұ����� ��� 0)

    private PlayerStatusHandler characterStats;
    private PlayerStat playerStat;
    Vector3 dir;

    private void Awake()
    {
        characterStats = GameManager.Instance.player.GetComponent<PlayerStatusHandler>();
    }
    private void Start()
    {
        Init();
        playerStat = characterStats.GetStat();
        dir = new Vector3(GameManager.Instance.lastPlayerController.facingDirection, 0, 0);

    }
    private void Update()
    {
        if(type == SkillType.Active) 
        StartCoroutine(SkillActivation());
    }

    public void Init()
    {
        if (curSkill == null) return;
        type = curSkill.Type;
        activeType = curSkill.ActiveType;
        skillName = curSkill.SkillName;
        skillIcon = curSkill.SkillIcon;
        efts = curSkill.SkillEffects;
        power = curSkill.Power;
        cost = curSkill.Cost;
        descriptiion = curSkill.Description;
        skillProperty = curSkill.SkillProperty;
        price = curSkill.Price;
    }

    public bool Use()
    {
        // Debug.Log("1"+playerStat.mana);
        //
        // if (playerStat.mana <= 0) return false;
        // Debug.Log("2"+playerStat.mana);
       // if (CostDecrease()) return false;

        bool isUsed = false;

        foreach (SkillEffect eft in efts)
        {

            isUsed = eft.ExcuteRole(power, type);
        }

        if (isUsed && type != SkillType.Buff)
        {
            //CostDecrease();
        }


        return isUsed; // ��ų ��� ���� ����
    }

    // bool CostDecrease()
    // {
    //     if (playerStat.mana >= cost)
    //     {
    //         playerStat.mana -= cost;
    //         return true;
    //     }
    //     else {
    //         return false;
    //     }
    //
    // }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyStatusHandler>().TakeDamage(power + playerStat.spellPower);
            //power�� �÷��̾� �ֹ����� ������� �������� �� collision.getComponent<Enemy>().TakeDamage??
            if (activeType)
            {
                Destroy(gameObject);
            }
        }

    }

    IEnumerator SkillActivation()
    {
        if (dir.x > 0)
        {
            transform.GetComponent<SpriteRenderer>().flipX = activeType;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().flipX = !activeType;

        }

        if (activeType)
        {
            transform.Translate(new Vector3(dir.x
                * 20 * Time.deltaTime, 0, 0)); // 1f�� �÷��̾ �ٶ󺸴� ���⿡ ���� -1f or 1f
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            // ����ǰ� 1�� �� �����
        }
        Destroy(gameObject);

        yield return null;
    }

    #region ������Ƽ
    public SkillSO CurSkill { get { return curSkill;  } set { curSkill = value;  } }
    public SkillType Type { get { return type; } }
    public bool ActiveType { get { return activeType; } set { activeType = value; } }
    public string SkillName { get { return skillName; } set { skillName = value; } }
    public Sprite SkillIcon { get { return skillIcon; } set { skillIcon = value; } }
    public List<SkillEffect> Efts { get { return efts; } set { efts = value; } }
    public int Power { get { return power; } set { power = value; } }
    public int Cost { get { return cost; } set { cost = value; } }
    public List<String> description { get { return descriptiion; } set { descriptiion = value; } }
    public PropertyType SkillProperty { get { return skillProperty; } }
    public int Price { get { return price; } set { price = value; } }
    #endregion
}
