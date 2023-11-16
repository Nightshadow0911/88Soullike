using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSentence : MonoBehaviour
{
    public string[] sentences;
    public Transform chatTr;
    public GameObject chatBoxPrefab;
    public string npcName;
    public bool isInteractable = false;
    private CharacterInfoUI charInfoUI;
    private InventoryUI invenUI;

    private void Start()
    {
        charInfoUI = CharacterInfoUI.instance;
        invenUI = InventoryUI.instance;
    }


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
            if(npcName.Equals("Stat")) charInfoUI.growPopupBtn.GetComponent<Button>().interactable = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInteractable = false;
            if (npcName.Equals("Stat"))
            {
                charInfoUI.growthPopup.SetActive(false);
                charInfoUI.growPopupBtn.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void InteractWithNPC(string npcName)
    {
        switch (npcName)
        {
            case "Shop":
                invenUI.shopPanel.SetActive(true);
                break;
            case "Stat":
                charInfoUI.growPopupBtn.SetActive(true);
                break;
            default:
                invenUI.shopPanel.SetActive(true);
                break;


        }
    }
}
