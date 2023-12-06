using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FullScreenUI : MonoBehaviour
{
    int uiIndex; // uiIndex는 현재 가장 높은 UI의 인덱스 예시로 처음 3개짜리 UI가 생성되면 uiIndex는 3이고 이후 3번째 UI를 추가하는 역할
    public event Action OnFocus;

    public void ChangeIndex(int index) // index는 List에서 바꿀 UI, SwitchPanel.cs 만들면서 다시 볼 것
    {
        OnFocus();
        uiIndex++;
    }  
}
