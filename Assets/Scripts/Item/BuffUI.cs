using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffUI : MonoBehaviour
{
    public string type;
    public float percentage;
    public float duration;
    public float currentTime;
    public Image icon;

    private void Awake()
    {
        icon = GetComponent<Image>();
    }

    public void Init(string type, float per, float du)
    {
        this.type = type;
        percentage = per;
        duration = du;
        currentTime = duration;
    }
    WaitForSeconds seconds = new WaitForSeconds(0.1f);
    public void Excute()
    {
        StartCoroutine(Activation());
    }

    IEnumerator Activation()
    {
        while(currentTime > 0)
        {
            currentTime -= 0.1f;
            yield return seconds;
        }
        currentTime = 0;
        DeActivation();
    }

    public void DeActivation()
    {
        Destroy(gameObject);
    }
}
