using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FullScreenUI : MonoBehaviour
{
    int uiIndex; // uiIndex�� ���� ���� ���� UI�� �ε��� ���÷� ó�� 3��¥�� UI�� �����Ǹ� uiIndex�� 3�̰� ���� 3��° UI�� �߰��ϴ� ����
    public event Action OnFocus;

    public void SetFirst() // index�� List���� �ٲ� UI, SwitchPanel.cs ����鼭 �ٽ� �� ��
    {
        OnFocus();
    }  
}
