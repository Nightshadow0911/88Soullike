using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    public string statName;
    public int value;
    public float duration;
    public float currentTime;
    public Image icon;

    private void Awake()
    {
        icon = GetComponent<Image>();
    }

    public void Init(string statName, int value, float du, Sprite icon)
    {
        this.statName = statName;
        this.value = value;
        duration = du;
        currentTime = duration;
        this.icon.sprite = icon;
    }
    WaitForSeconds seconds = new WaitForSeconds(1f);
    public void Excute()
    {
        StartCoroutine(Activation());
    }

    IEnumerator Activation()
    {
        while(currentTime > 0)
        {
            currentTime -= 1f;
            yield return seconds;

            if(currentTime < (duration * 0.2f))
            {
                Color fadedColor = icon.color;
                //fadedColor.a = fadedColor.a > 0.5f ? 0.2f : 1f;
                fadedColor.a = 0.5f;
                icon.color = fadedColor;
            }
        }
        currentTime = 0;
        DeActivation();
    }

    public void DeActivation()
    {
        Destroy(gameObject);
    }
}
