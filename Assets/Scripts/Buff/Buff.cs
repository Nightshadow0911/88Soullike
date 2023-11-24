using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class Buff : MonoBehaviour
{
    public BuffSO buff;
    
    public void Activated(object obj, Action cbDone) // obj = 버프가 적용될 객체, cbDone 버프가 끝났음을 obj에게 알리기 위함
    {
        //StartCoroutine(SC_Timer(obj, cbDone));
        StartCoroutine(SC_Timer2(obj, cbDone));
    }

    protected IEnumerator SC_Timer2(object obj, Action cbDone)
    {
        var fi = obj.GetType().GetTypeInfo().GetDeclaredProperty(buff.StatName); // Reflection으로 obj에 선언된 프로퍼티를 가져옴

        int v = (int)fi.GetValue(obj); // 현재 값 => 현재값으로 하니까 여러개 먹었을때 버그

        int buffed = v + buff.Value; // 버프가 적용된 값

        fi.SetValue(obj, buffed); // obj에 버프된 스탯을 적용

        float elapsed = 0; // 버프 시간(durTime)동안 대기, 버프 지속시간

        while (elapsed <= buff.DurTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        fi.SetValue(obj, v); // obj의 값을 원래대로 되돌림
        cbDone.Invoke(); // 버프가 끝났음을 알림
    }

    protected IEnumerator SC_Timer(object obj, Action cbDone)
    {
        var fi = obj.GetType().GetTypeInfo().GetDeclaredField(buff.StatName); // Reflection으로 obj에 선언된 변수를 가져옴

        int v = (int)fi.GetValue(obj); // 현재 값

        int buffed = v + buff.Value; // 버프가 적용된 값

        fi.SetValue(obj, buffed); // obj에 버프된 스탯을 적용

        float elapsed = 0; // 버프 시간(durTime)동안 대기, 버프 지속시간

        while(elapsed <= buff.DurTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        fi.SetValue(obj, v); // obj의 값을 원래대로 되돌림
        cbDone.Invoke(); // 버프가 끝났음을 알림
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Buff otherBuff = (Buff)obj;

        // 여기에 버프 객체의 내용을 비교하는 로직을 구현합니다.
        // 예시로서 버프의 이름을 비교하는 경우:
        return buff.StatName == otherBuff.buff.StatName;
    }

    public override int GetHashCode()
    {
        return buff.name.GetHashCode();
    }
}
