using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/SkillEft/ActiveEft")]
public class ActiveSkillEffect : SkillEffect
{
    [SerializeField] private bool type; // false �ٰŸ�, true ���Ÿ�
    public GameObject skillPrefab; // false �ٰŸ�, true ���Ÿ�
    public Vector3 skillPosition;
    public override bool ExcuteRole(int power, SkillType type) 
    {
        skillPosition = GameManager.instance.playerAttack.attackPoint.transform.position;
        GameObject go = Instantiate(skillPrefab, skillPosition, Quaternion.identity);

        return true;
    }

}
