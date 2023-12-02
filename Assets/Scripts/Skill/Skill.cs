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
    [SerializeField] private int power; // 데미지 계산식(ex: 스킬공격력 * power) or 버프시간 등
    [SerializeField] private int cost; // 마나 코스트
    [SerializeField] private List<String> descriptiion;
    [SerializeField] private PropertyType skillProperty;
    [SerializeField] private int price; // 가격(상점에서 살때의 가격임, 상점판매가 불가능한 경우 0)

    CharacterStats characterStats;
    Vector3 dir;
    private void Start()
    {
        Init();
        characterStats = GameManager.Instance.playerStats;
        dir = new Vector3(GameManager.Instance.lastPlayerController.facingDirection, 0, 0);

    }
    private void Update()
    {
        StartCoroutine(move());
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
        if (characterStats.characterMana <= 0) return false;

        bool isUsed = false;

        foreach (SkillEffect eft in efts)
        {

            isUsed = eft.ExcuteRole(power, type);
        }

        if (isUsed)
        {
            CostDecrease();
        }


        return isUsed; // 스킬 사용 성공 여부
    }

    void CostDecrease()
    {
        if (characterStats.characterMana >= cost)
        {
            characterStats.characterMana -= cost;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<skeletonEnemy>().TakeDamage(power + characterStats.NormalSkillDamage);
            //power와 플레이어 주문력을 기반으로 데미지를 줌 collision.getComponent<Enemy>().TakeDamage??
            if (activeType)
            {
                Destroy(gameObject);
            }
        }

    }

    IEnumerator move()
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
                * 20 * Time.deltaTime, 0, 0)); // 1f는 플레이어가 바라보는 방향에 따라 -1f or 1f
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            // 실행되고 1초 뒤 사라짐
        }
        Destroy(gameObject);

        yield return null;
    }

    #region 프로퍼티
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
