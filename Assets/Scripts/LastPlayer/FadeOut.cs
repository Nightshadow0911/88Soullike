using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float FadeOutDeley;
    private float FadeOutDelaySeconds;
    public GameObject fadeOut;
    public bool makeFadeOut = false;
    // Start is called before the first frame update
    void Start()
    {
        FadeOutDelaySeconds = FadeOutDeley;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeFadeOut)
        {
            if (FadeOutDelaySeconds>0)
            {
                FadeOutDelaySeconds -= Time.deltaTime;

            }else
            {
                GameObject currentGhost = Instantiate(fadeOut, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                FadeOutDelaySeconds = FadeOutDeley;
                Destroy(currentGhost, 1f);
            }

        }
    }
}
