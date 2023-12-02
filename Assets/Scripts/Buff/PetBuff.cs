using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBuff : MonoBehaviour
{
    public Skill petSkill;
    public List<SkillSO> petSkills = new List<SkillSO>();
    WaitForSeconds seconds;
    private void Start()
    {
        seconds = new WaitForSeconds(30f);
        StartCoroutine(RepeatSkillTimer());
    }

    IEnumerator RepeatSkillTimer()
    {
        while(true)
        {
            yield return StartCoroutine(UseSkill());
            yield return seconds;
        }
    }

    IEnumerator UseSkill()
    {
        petSkill.CurSkill = petSkills[Random.Range(0, petSkills.Count)];
        petSkill.Init();
        petSkill.Use();
        Debug.Log("½ΊΕ³Έν" + petSkill.SkillName);

        yield return null;
    }
}
