using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Scriptable", menuName = "Scriptable/Buff")]
public class BuffSO : ScriptableObject
{
    [SerializeField] private float durTime;
    [SerializeField] private int value;
    [SerializeField] private string statName;
    [SerializeField] private Sprite buffImage;
    

    public float DurTime { get { return durTime; } }
    public int Value { get { return value; } }
    public string StatName { get { return statName; } }
    public Sprite BuffImage { get { return buffImage; } }
}
