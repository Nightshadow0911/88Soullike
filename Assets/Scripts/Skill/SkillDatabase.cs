using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDatabase : MonoBehaviour
{
    public static SkillDatabase instance;
    private void Awake()
    {
        instance = this;
    }
    public List<SkillSO> skillDB = new List<SkillSO>();

    private void Start()
    {

    }
}
