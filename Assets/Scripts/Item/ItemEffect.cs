using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public abstract bool ExcuteRole(int power); // 아이템별 다른 효과
}
