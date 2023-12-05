using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class Buff : MonoBehaviour
{
    public BuffSO buff;

    public void Activated(object obj, Action cbDone) // obj = ������ ����� ��ü, cbDone ������ �������� obj���� �˸��� ����
    {
        StartCoroutine(SC_Timer(obj, cbDone));
    }

    protected IEnumerator SC_Timer2(object obj, Action cbDone)
    {
        var fi = obj.GetType().GetTypeInfo().GetDeclaredProperty(buff.StatName); // Reflection���� obj�� ����� ������Ƽ�� ������

        int v = (int)fi.GetValue(obj); // ���� �� => ���簪���� �ϴϱ� ������ �Ծ����� ����

        int buffed = v + buff.Value; // ������ ����� ��

        fi.SetValue(obj, buffed); // obj�� ������ ������ ����

        float elapsed = 0; // ���� �ð�(durTime)���� ���, ���� ���ӽð�

        while (elapsed <= buff.DurTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        fi.SetValue(obj, v); // obj�� ���� ������� �ǵ���
        cbDone.Invoke(); // ������ �������� �˸�
    }

    protected IEnumerator SC_Timer(object obj, Action cbDone)
    {
        var fi = obj.GetType().GetTypeInfo().GetDeclaredField(buff.StatName); // Reflection���� obj�� ����� ������ ������

        int v = (int)fi.GetValue(obj); // ���� ��

        int buffed = v + buff.Value; // ������ ����� ��

        fi.SetValue(obj, buffed); // obj�� ������ ������ ����

        float elapsed = 0; // ���� �ð�(durTime)���� ���, ���� ���ӽð�

        while (elapsed <= buff.DurTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        fi.SetValue(obj, v); // obj�� ���� ������� �ǵ���
        cbDone.Invoke(); // ������ �������� �˸�
    }



    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Buff otherBuff = (Buff)obj;

        // ���� ��ü�� ������ ��
        // ���÷μ� ������ �̸��� ���ϴ� ���:
        return buff.StatName == otherBuff.buff.StatName;
    }

    public override int GetHashCode()
    {
        return buff.name.GetHashCode();
    }
}
