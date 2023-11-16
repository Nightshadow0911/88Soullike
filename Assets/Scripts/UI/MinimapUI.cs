using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapUI : MonoBehaviour
{
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private Transform top;
    [SerializeField] private Transform bottom;

    [SerializeField] private Image minimapImage;
    [SerializeField] private Image minimapPlayerImage;

    [SerializeField] private LastPlayerController targetPlayer;

    private void Start()
    {
        //var inst = Instantiate(minimapImage.material);
        //minimapImage.material = inst;

        targetPlayer = GameManager.Instance.lastPlayerController;
    }

    private void Update()
    {
        if(targetPlayer != null)
        {
            Vector2 mapArea = new Vector2(Vector3.Distance(left.position, right.position), Vector3.Distance(bottom.position, top.position));
            Vector2 charPos = new Vector2(Vector3.Distance(left.position, new Vector3(targetPlayer.transform.position.x, 0f, 0f)),
                Vector3.Distance(bottom.position, new Vector3(0f, targetPlayer.transform.position.y, 0f)));
            
            Vector2 normalizedPos = new Vector2(charPos.x / mapArea.x, charPos.y / mapArea.y);
            Vector2 playerImageOffset = new Vector2(minimapImage.rectTransform.sizeDelta.x * normalizedPos.x - minimapImage.rectTransform.sizeDelta.x * 0.5f, minimapImage.rectTransform.sizeDelta.y * normalizedPos.y - minimapImage.rectTransform.sizeDelta.y * 0.5f);
            minimapPlayerImage.rectTransform.anchoredPosition = playerImageOffset;

            //Vector2 normalPos = new Vector2(charPos.x / mapArea.x, charPos.y / mapArea.y);

            //minimapPlayerImage.rectTransform.anchoredPosition = new Vector2(minimapImage.rectTransform.sizeDelta.x * normalPos.x, minimapImage.rectTransform.sizeDelta.y * normalPos.y);

            //Vector2 playerImageOffset = new Vector2(minimapImage.rectTransform.sizeDelta.x * normalPos.x, minimapImage.rectTransform.sizeDelta.y * normalPos.y);
            //minimapPlayerImage.rectTransform.anchoredPosition = playerImageOffset + new Vector2(minimapImage.rectTransform.rect.width * minimapImage.rectTransform.pivot.x, minimapImage.rectTransform.rect.height * minimapImage.rectTransform.pivot.y);
        }
    }
}
