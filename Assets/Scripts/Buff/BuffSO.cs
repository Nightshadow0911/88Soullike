using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/Buff")]
public class BuffSO : ScriptableObject
{
    [SerializeField] private float durTime;
    [SerializeField] private int value;
    [SerializeField] private string statName;

    public float DurTime { get { return durTime; } }
    public int Value { get { return value; } }
    public string StatName { get { return statName; } }
}
