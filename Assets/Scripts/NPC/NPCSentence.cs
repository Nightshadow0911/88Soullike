using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSentence : MonoBehaviour
{
    public string[] sentences;
    public Transform chatTr;
    public GameObject chatBoxPrefab;
    public string npcName;
    public bool isInteractable = false;

    public void TalkNpc()
    {
        GameObject go = Instantiate(chatBoxPrefab);
        go.GetComponent<ChatSystem>().Ondialogue(sentences, chatTr);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            isInteractable = true;
            collision.GetComponent<Inventory>().currentNPC = this;
            TalkNpc();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInteractable = false;
        }
    }

    public void InteractWithNPC(string npcName)
    {
        switch (npcName)
        {
            case "Shop":
                InventoryUI.instance.shopPanel.SetActive(true);
                break;
            default:
                InventoryUI.instance.shopPanel.SetActive(true);
                break;


        }
    }
}
