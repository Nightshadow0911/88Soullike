using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/SkillEft/PassiveEft")]
public class PassiveSkillEffect : SkillEffect
{
    // �нú�� �ø� �÷��̾� ����
    [SerializeField] private string selectStat;
    // �ٸ� Ư��ȿ���� ������ like �Ӽ� ����
    public override bool ExcuteRole(int power, SkillType type)
    {
        // for (int i = 0; i < power; i++) // �÷��̾� ������ �ѹ��� ������Ű�� �޼��尡 ��� �̷��� ����
        // {
        //     GameManager.Instance.playerStats.TryLevelUp(selectStat);
        //
        // }

        return true;
    }
}
