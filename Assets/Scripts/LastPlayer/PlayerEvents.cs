using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerEvents : MonoBehaviour
{

    public static UnityAction<GameObject, int> playerDamaged;
    public static UnityAction<GameObject, int> playerHealed;
    public static UnityAction<GameObject, int> monsterDamaged;
}
